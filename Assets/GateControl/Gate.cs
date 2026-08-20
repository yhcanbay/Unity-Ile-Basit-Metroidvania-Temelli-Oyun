using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private string level;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            player.gatherInput.DisablePlayerMap();
            player.physicsControl.ResetVelocity();
            LevelManager.instance.ChangeLevel(level);

            GetComponent<Collider2D>().enabled = false;
        }
    }
}
