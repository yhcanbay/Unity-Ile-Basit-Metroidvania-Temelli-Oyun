using UnityEngine;

public class MoveAbility : BaseAbility {
    private string MoveAnimParameterName = "Move";
    private int MoveParameterInt;

    [SerializeField] private float speed;
    protected override void Initialization()
    {
        base.Initialization();
        MoveParameterInt = Animator.StringToHash(MoveAnimParameterName);
    }
    public override void EnterAbility()
    {
        player.Flip();
    }
    public override void ProcessAbility()
    {
        player.Flip();
        if(linkedPhysicsControl.grounded && linkedInput.horizontalInput == 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if (!linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocity = new Vector2(speed * linkedInput.horizontalInput, linkedPhysicsControl.rb.linearVelocityY);
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(MoveParameterInt, PlayerStates.State.Run == linkedStateMachine.currentState);
    }
}
