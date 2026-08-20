using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbAbility : BaseAbility
{
    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float minLadderTime;
    private float startMinLadderTime;

    public bool canGoOnLadder;

    private string ladderAnimParameterName = "Ladder";
    private int ladderParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        startMinLadderTime = minLadderTime;
        ladderParameterInt = Animator.StringToHash(ladderAnimParameterName);
    }
    private void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
    }
    private void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
    }
    public override void EnterAbility()
    {
        linkedPhysicsControl.ResetVelocity();
        linkedPhysicsControl.DisableGravity();
    }
    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
    }
    public override void ProcessAbility()
    {
        minLadderTime -= Time.deltaTime;
        if (minLadderTime < 0)
        {
            if (linkedPhysicsControl.grounded)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            if (!canGoOnLadder)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocityY = linkedInput.verticalInput * climbSpeed;
        linkedPhysicsControl.rb.linearVelocityX = 2f * linkedInput.horizontalInput * (float) Math.Abs(linkedInput.verticalInput);
    }
    private void TryToClimb(InputAction.CallbackContext context)
    {
        if (!isPermitted || linkedStateMachine.currentState==PlayerStates.State.Knockback)
        {
            return;
        }
        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            return;
        }
        if (canGoOnLadder)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
            minLadderTime = startMinLadderTime;
        }

    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderParameterInt, linkedStateMachine.currentState == PlayerStates.State.Ladders);
        // verticalInput eksi de olsa, artı da olsa hız her zaman pozitiftir (Mathf.Abs)
        linkedAnimator.SetFloat("ClimbVelocity", Mathf.Abs(linkedInput.verticalInput));
    }
}
