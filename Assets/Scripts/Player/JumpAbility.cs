using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAbility : BaseAbility
{
    [SerializeField] private float airspeed;
    [SerializeField] private float jumpForce;
    [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float minimumAirTime;
    private float startMinimumAirTime;

    public InputActionReference jumpActionRef;

    private string JumpAnimParameterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int JumpParameterInt;
    private int ySpeedParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirTime;
        JumpParameterInt = Animator.StringToHash(JumpAnimParameterName);
        ySpeedParameterInt = Animator.StringToHash(ySpeedAnimParameterName);
    }
    private void OnEnable()
    {
        jumpActionRef.action.performed += tryToJump;
        jumpActionRef.action.canceled += stopJump;
    }

    private void OnDisable()
    {

        jumpActionRef.action.performed -= tryToJump;
        jumpActionRef.action.canceled -= stopJump;

    }
    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTime -= Time.deltaTime;
        if(linkedPhysicsControl.grounded && minimumAirTime < 0)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }else if(linkedPhysicsControl.wallDetected && minimumAirTime < 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
        }
    }
    public override void ProcessFixedAbility()
    {
        if (!linkedPhysicsControl.grounded)
        {
            linkedPhysicsControl.rb.linearVelocity = new Vector2(airspeed * linkedInput.horizontalInput, linkedPhysicsControl.rb.linearVelocityY);
        }
    }
    private void stopJump(InputAction.CallbackContext value)
    {
        if (!linkedPhysicsControl.grounded && linkedPhysicsControl.rb.linearVelocityY > 0)
        {
            float cutY = linkedPhysicsControl.rb.linearVelocityY * jumpCutMultiplier;
            linkedPhysicsControl.rb.linearVelocity = new Vector2(linkedPhysicsControl.rb.linearVelocityX, cutY);
        }
    }
    private void tryToJump(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback)
        {
            return;
        }
        if (linkedPhysicsControl.coyoteTimer >= 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocity = new Vector2(airspeed * linkedInput.horizontalInput,jumpForce);
            minimumAirTime = startMinimumAirTime;
            linkedPhysicsControl.coyoteTimer = -1;
        }
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(JumpParameterInt, PlayerStates.State.Jump == linkedStateMachine.currentState || PlayerStates.State.WallJump == linkedStateMachine.currentState || PlayerStates.State.DoubleJump == linkedStateMachine.currentState);
        linkedAnimator.SetFloat(ySpeedParameterInt, linkedPhysicsControl.rb.linearVelocityY);
    }
}
