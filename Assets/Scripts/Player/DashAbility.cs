using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : BaseAbility
{
    private string dashAnimParameterName = "Dash";
    private int dashParameterInt;

    [SerializeField] private float dashForce;
    [SerializeField] private float minDashTime;
    private float startMinDashTime;

    public InputActionReference dashActionRef;
    protected override void Initialization()
    {
        base.Initialization();
        dashParameterInt = Animator.StringToHash(dashAnimParameterName);
        startMinDashTime = minDashTime;
    }
    private void OnEnable()
    {
        dashActionRef.action.performed += TryToDash;
    }
    private void OnDisable()
    {
        dashActionRef.action.performed -= TryToDash;
    }
    public override void ExitAbility()
    {
        player.playerStats.setCanTakeDamage(true);
        linkedPhysicsControl.EnableGravity();
        linkedPhysicsControl.ResetVelocity(); 
    }
    public override void ProcessAbility()
    {
        minDashTime -= Time.deltaTime;
        if(minDashTime < 0)
        {
            if (linkedPhysicsControl.grounded)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }
    }
    private void TryToDash(InputAction.CallbackContext context)
    {
        if (!isPermitted || !linkedPhysicsControl.getCanAirDash() || linkedStateMachine.currentState == PlayerStates.State.Knockback)
        {
            return;
        }
        if(linkedStateMachine.currentState == PlayerStates.State.Dash)
        {
            return;
        }
        if (linkedStateMachine.currentState == PlayerStates.State.WallSlide)
        {
            player.ForceFlip();
        }
        linkedStateMachine.ChangeState(PlayerStates.State.Dash);
        player.playerStats.setCanTakeDamage(false);
        linkedPhysicsControl.DisableGravity();
        linkedPhysicsControl.ResetVelocity();
        minDashTime = startMinDashTime;
        if (player.FacingRight)
        {
            linkedPhysicsControl.rb.linearVelocityX = dashForce;
        }else if (!player.FacingRight)
        {
            linkedPhysicsControl.rb.linearVelocityX = -dashForce;
        }
        if(!linkedPhysicsControl.grounded || !linkedPhysicsControl.wallDetected)
        {
            linkedPhysicsControl.setCanAirDash(false);
        }
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashParameterInt, PlayerStates.State.Dash == linkedStateMachine.currentState);
    }
}
