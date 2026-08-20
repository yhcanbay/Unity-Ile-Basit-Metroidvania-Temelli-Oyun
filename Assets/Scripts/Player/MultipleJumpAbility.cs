using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultipleJumpAbility : BaseAbility
{
    [SerializeField] private float airspeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float minimumAirTime;
    private float startMinimumAirTime;

    [SerializeField] private int totalJumpCount;
    private int jumpCount;

    [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;

    public InputActionReference jumpActionRef;

    private string JumpAnimParameterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int JumpParameterInt;
    private int ySpeedParameterInt;
    protected override void Initialization()
    {
        base.Initialization();
        JumpParameterInt = Animator.StringToHash(JumpAnimParameterName);
        ySpeedParameterInt = Animator.StringToHash(ySpeedAnimParameterName);
        jumpCount = totalJumpCount;
    }
    private void OnEnable()
    {
        jumpActionRef.action.performed += TryDoubleJump;
        jumpActionRef.action.canceled += TryStopDoubleJump;
    }
    private void OnDisable()
    {
        jumpActionRef.action.performed -= TryDoubleJump;
        jumpActionRef.action.canceled -= TryStopDoubleJump;
    }
    public override void EnterAbility()
    {
        linkedPhysicsControl.rb.linearVelocity = new Vector2(airspeed * linkedInput.horizontalInput, jumpForce);
        minimumAirTime = startMinimumAirTime;
        jumpCount--;
    }
    public override void ExitAbility()
    {
        if(linkedPhysicsControl.grounded || linkedPhysicsControl.wallDetected)
            jumpCount = totalJumpCount;
    }
    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTime -= Time.deltaTime;
        if (linkedPhysicsControl.grounded && minimumAirTime < 0)
        {
            if (linkedInput.horizontalInput != 0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        else if (linkedPhysicsControl.wallDetected && minimumAirTime < 0)
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
    private void TryDoubleJump(InputAction.CallbackContext context)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.Knockback)
        {
            return;
        }
        if (linkedPhysicsControl.grounded)
        {
            jumpCount = totalJumpCount;
        }
        if(jumpCount <= 0 || linkedPhysicsControl.grounded)
        {
            return;
        }
        if (linkedStateMachine.currentState != PlayerStates.State.Jump && linkedStateMachine.currentState != PlayerStates.State.DoubleJump)
        {
            return;
        }
        if (linkedStateMachine.currentState == PlayerStates.State.Jump)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.DoubleJump);
        }
        
    }
    private void TryStopDoubleJump(InputAction.CallbackContext context)
    {
        if (!linkedPhysicsControl.grounded && linkedPhysicsControl.rb.linearVelocityY > 0)
        {
            float cutY = linkedPhysicsControl.rb.linearVelocityY * jumpCutMultiplier;
            linkedPhysicsControl.rb.linearVelocity = new Vector2(linkedPhysicsControl.rb.linearVelocityX, cutY);
        }
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(JumpParameterInt, PlayerStates.State.DoubleJump == linkedStateMachine.currentState);
        linkedAnimator.SetFloat(ySpeedParameterInt, linkedPhysicsControl.rb.linearVelocityY);
    }
}
