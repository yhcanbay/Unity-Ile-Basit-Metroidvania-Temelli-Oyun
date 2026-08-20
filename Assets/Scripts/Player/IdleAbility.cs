using UnityEngine;

public class IdleAbility : BaseAbility
{
    private string IdleAnimParameterName = "Idle";
    private int idleParameterInt;
    public override void EnterAbility()
    {
        linkedPhysicsControl.rb.linearVelocityX = 0;
    }
    protected override void Initialization()
    {
        base.Initialization();
        idleParameterInt = Animator.StringToHash(IdleAnimParameterName);
    }
    public override void ProcessAbility()
    {
        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Run);
            player.Flip();
        }
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParameterInt, PlayerStates.State.Idle == linkedStateMachine.currentState);
    }
}
