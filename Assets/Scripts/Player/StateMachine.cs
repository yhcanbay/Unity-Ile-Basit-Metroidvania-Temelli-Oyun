using System;
using UnityEngine;

[Serializable]
public class StateMachine
{
    public PlayerStates.State previousState;
    public PlayerStates.State currentState;
    public BaseAbility[] arrayOfAbilities;

    public void ChangeState(PlayerStates.State state)
    {
        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == state && !ability.isPermitted)
            {
                return;
            }

        }

        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if(ability.thisAbilityState == currentState)
            {
                previousState = ability.thisAbilityState;
                ability.ExitAbility();
            }
        }

        foreach(BaseAbility ability in arrayOfAbilities)
        {
            if(ability.thisAbilityState == state && ability.isPermitted)
            {
                ability.EnterAbility();
                currentState = ability.thisAbilityState;
                break;
            }
            
        }
    }

    public void ForceChange(PlayerStates.State state)
    {
        previousState = currentState;
        currentState = state;
    }

}
