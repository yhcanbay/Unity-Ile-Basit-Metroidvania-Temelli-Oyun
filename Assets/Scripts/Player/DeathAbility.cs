using UnityEngine;

public class DeathAbility : BaseAbility
{
    private string deathAnimParameterName = "Death";
    private int deathParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        deathParameterInt = Animator.StringToHash(deathAnimParameterName);
    }

    public override void EnterAbility()
    {
        linkedPhysicsControl.ResetVelocity();
        linkedInput.DisablePlayerMap();
    }

    public void RestartGame()
    {
        LevelManager.instance.RestartGame();
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(deathParameterInt,player.stateMachine.currentState==PlayerStates.State.Death);
    }
}
