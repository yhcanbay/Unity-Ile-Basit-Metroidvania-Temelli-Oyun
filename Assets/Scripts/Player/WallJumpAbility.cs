using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpAbility : BaseAbility
{

    public InputActionReference wallJumpActionRef;

    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float minimumAirTime;
    [SerializeField] private float maximumAirTime;
    private float startMinimumAirTime;
    private float startMaximumAirTime;

    private void OnEnable()
    {
        wallJumpActionRef.action.performed += TryToWallJump;
    }
    private void OnDisable()
    {
        wallJumpActionRef.action.performed -= TryToWallJump;
    }
    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirTime;
        startMaximumAirTime = maximumAirTime;
    }
    public override void ProcessAbility()
    {
        minimumAirTime -= Time.deltaTime;
        maximumAirTime -= Time.deltaTime;

        if(minimumAirTime <= 0 && linkedPhysicsControl.grounded)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        
        if (maximumAirTime <= 0)
        {
            if (linkedPhysicsControl.grounded)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
            return;
        }
        if (linkedPhysicsControl.wallDetected && minimumAirTime < 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            minimumAirTime = -1;
        }
    }
    private void TryToWallJump(InputAction.CallbackContext value)
    {
        if (!isPermitted)
        {
            return;
        }
        if (linkedPhysicsControl.wallDetected && !linkedPhysicsControl.grounded) {
            linkedStateMachine.ChangeState(PlayerStates.State.WallJump);
            maximumAirTime = startMaximumAirTime;
            minimumAirTime = startMinimumAirTime;

            player.ForceFlip();

            if (player.FacingRight)
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            }
            else
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);
            }
        }
    }
}
