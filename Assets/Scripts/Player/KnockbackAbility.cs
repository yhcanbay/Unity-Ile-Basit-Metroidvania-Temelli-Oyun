using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnockbackAbility : BaseAbility
{
    private string KnockbackAnimParameterName = "Knockback";
    private int KnockbackParameterInt;

    private Coroutine currentKnockback;

    private float jumpingRate;
    private float totalDuration;
    private bool recoveryStarted;

    [SerializeField] private float knockbackRecoveryAirspeed;

    [SerializeField] private InputActionReference jumpActionRef;

    protected override void Initialization()
    {
        base.Initialization();
        KnockbackParameterInt = Animator.StringToHash(KnockbackAnimParameterName);
    }
    private void OnEnable()
    {
        jumpActionRef.action.performed += tryToJump;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= tryToJump;
    }

    public override void ProcessFixedAbility()
    {
        if (!recoveryStarted) return;

        float controlFactor = jumpingRate * jumpingRate * jumpingRate; // ease-in cubic
        float desiredX = linkedInput.horizontalInput * knockbackRecoveryAirspeed;

        linkedPhysicsControl.rb.linearVelocityX = Mathf.Lerp(linkedPhysicsControl.rb.linearVelocityX, desiredX, controlFactor);

        jumpingRate += Time.fixedDeltaTime / totalDuration;
        jumpingRate = Mathf.Clamp01(jumpingRate);
    }

    public void StartKnockback(float duration, Vector2 force, Transform enemyObject)
    {
        if (!player.playerStats.GetCanTakeDamage())
            return;
        if (currentKnockback == null)
        {
            currentKnockback = StartCoroutine(Knockback(duration, force, enemyObject, 0));
        }
        else
        {
            StopCoroutine(currentKnockback);
            currentKnockback = StartCoroutine(Knockback(duration, force, enemyObject, 0));
        }
    }

    public void StartKnockback(float duration, Vector2 force, Transform enemyObject, int pushDirection)
    {
        if (!player.playerStats.GetCanTakeDamage())
            return;
        if (currentKnockback == null)
        {
            currentKnockback = StartCoroutine(Knockback(duration, force, enemyObject, pushDirection));
        }
        else
        {
            StopCoroutine(currentKnockback);
            currentKnockback = StartCoroutine(Knockback(duration, force, enemyObject, pushDirection));
        }
    }

    private IEnumerator Knockback(float duration, Vector2 force, Transform enemyObject, int pushDirection)
    {
        totalDuration = duration;
        recoveryStarted = false;
        linkedStateMachine.ChangeState(PlayerStates.State.Knockback);
        linkedPhysicsControl.ResetVelocity();
        jumpingRate = 0f;

        if (pushDirection == 0)
        {
            if (transform.position.x > enemyObject.position.x)
            {
                linkedPhysicsControl.rb.linearVelocity = force;
            }
            else
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(-force.x, force.y);
            }
        }
        else
        {
            force.x *= pushDirection;
            linkedPhysicsControl.rb.linearVelocity = force;
        }

        yield return new WaitForSeconds(duration);

        // Duration bitti, recovery başlıyor
        recoveryStarted = true;

        if (player.playerStats.GetHealth() <= 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
            yield break;
        }

        while (!linkedPhysicsControl.grounded)
        {
            if (player.playerStats.GetHealth() <= 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Death);
                yield break;
            }
            yield return null;
        }

        linkedStateMachine.ChangeState(
            linkedInput.horizontalInput != 0 ? PlayerStates.State.Run : PlayerStates.State.Idle);
    }

    private void tryToJump(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState != PlayerStates.State.Knockback)
        {
            return;
        }
        if (!recoveryStarted)
        {
            return;
        }
        linkedStateMachine.ChangeState(PlayerStates.State.DoubleJump);
        
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(KnockbackParameterInt, linkedStateMachine.currentState == PlayerStates.State.Knockback);
    }
}
