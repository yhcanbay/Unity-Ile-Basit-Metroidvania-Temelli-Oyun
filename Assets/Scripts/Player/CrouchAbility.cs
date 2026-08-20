using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAbility : BaseAbility
{
    [SerializeField] private float crouchSpeed;

    public InputActionReference crouchInputRef;

    [Header("Collider Settings - Standing")]
    [SerializeField] private Vector2 standingSize;
    [SerializeField] private Vector2 standingOffset;

    [Header("Collider Settings - Crouching")]
    [SerializeField] private Vector2 crouchSize;
    [SerializeField] private Vector2 crouchOffset;


    private string crouchAnimParameterName = "Crouch";
    private int crouchParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        crouchParameterInt = Animator.StringToHash(crouchAnimParameterName);
    }
    private void OnEnable()
    {
        crouchInputRef.action.performed += TryToCrouch;
        crouchInputRef.action.canceled += ExitToCrouch;
    }
    private void OnDisable()
    {
        crouchInputRef.action.performed -= TryToCrouch;
        crouchInputRef.action.canceled -= ExitToCrouch;
    }
    public override void EnterAbility()
    {
        linkedPhysicsControl.ResetVelocity();
        linkedPhysicsControl.SwitchCollider();
        player.playerStats.EnableCrounchStatCollider();
    }
    public override void ExitAbility()
    {
        linkedPhysicsControl.SwitchCollider();
        player.playerStats.EnableStandingStatCollider();
    }
    public override void ProcessAbility()
    {
        player.Flip();
        if (!linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocityX = linkedInput.horizontalInput * crouchSpeed;
    }
    private void ExitToCrouch(InputAction.CallbackContext context)
    {
        if(linkedStateMachine.currentState == PlayerStates.State.Crouch)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
    }

    private void TryToCrouch(InputAction.CallbackContext context)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback)
        {
            return;
        }
        if (!linkedPhysicsControl.grounded 
            || linkedStateMachine.currentState == PlayerStates.State.Dash
            || linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            return;
        }
        linkedStateMachine.ChangeState(PlayerStates.State.Crouch);
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(crouchParameterInt, linkedStateMachine.currentState == PlayerStates.State.Crouch);
    }
}
