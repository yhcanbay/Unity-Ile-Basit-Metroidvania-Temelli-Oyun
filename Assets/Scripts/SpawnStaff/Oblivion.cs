using UnityEngine;

public class Oblivion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            player.stateMachine.ChangeState(PlayerStates.State.Death);
        }
    }
}
