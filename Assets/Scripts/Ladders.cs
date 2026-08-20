using Unity.VisualScripting;
using UnityEngine;

public class Ladders : MonoBehaviour
{
    public ClimbAbility climbAbility;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        climbAbility = collision.GetComponent<ClimbAbility>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (climbAbility != null && climbAbility.isPermitted)
        {
            climbAbility.canGoOnLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (climbAbility != null && climbAbility.isPermitted)
        {
            climbAbility.canGoOnLadder = false;
        }
    }
}
