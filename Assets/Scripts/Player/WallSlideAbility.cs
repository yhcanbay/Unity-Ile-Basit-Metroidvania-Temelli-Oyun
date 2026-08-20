using UnityEngine;

public class WallSlideAbility : BaseAbility
{
    private string wallSlideAnimParameterName = "WallSlide";
    private int wallSlideParameterInt;

    [SerializeField] private float slideSpeed;
    protected override void Initialization()
    {
        base.Initialization();
        wallSlideParameterInt = Animator.StringToHash(wallSlideAnimParameterName);

    }
    public override void EnterAbility()
    {
        linkedPhysicsControl.rb.linearVelocity = Vector2.zero;
    }
    public override void ProcessAbility()
    {
        if (linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        else if (!linkedPhysicsControl.wallDetected) 
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }else if ((linkedInput.horizontalInput > 0 && !player.FacingRight) || (linkedInput.horizontalInput < 0 && player.FacingRight))
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }
    public override void ProcessFixedAbility()
    {
        float currentSpeed = linkedPhysicsControl.rb.linearVelocityY;
        linkedPhysicsControl.rb.linearVelocityY = Mathf.Clamp(currentSpeed, -slideSpeed, 1);
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(wallSlideParameterInt, linkedStateMachine.currentState == PlayerStates.State.WallSlide);
    }
}
