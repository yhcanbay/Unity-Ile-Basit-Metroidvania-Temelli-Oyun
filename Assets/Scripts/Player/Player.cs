using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public StateMachine stateMachine;
    public PhysicsControl physicsControl;
    public Animator anim;
    public PlayerStats playerStats;

    private BaseAbility[] playerAbilities;

    public bool FacingRight = true;

    private void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
    }

    private void Update()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        Debug.Log("Current state : " + stateMachine.currentState);
    }

    private void FixedUpdate()
    {
        foreach (BaseAbility ability in playerAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }
    public void Flip()
    {
        if (FacingRight == true && gatherInput.horizontalInput < 0)
        {
            transform.Rotate(0, 180, 0);
            FacingRight = false;
        }
        else if (FacingRight == false && gatherInput.horizontalInput > 0)
        {
            transform.Rotate(0, 180, 0);
            FacingRight = true;
        }
    }
    public void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        FacingRight = !FacingRight;
    }
}
