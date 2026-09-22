using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private string level;
    public SpawnData spawnDataForOtherLevel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnMode.spawnFromCheckpoint = false;
            SaveLoadManager.instance.SaveData(spawnDataForOtherLevel, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
            Player player = collision.GetComponent<Player>();

            player.gatherInput.DisablePlayerMap();
            player.physicsControl.ResetVelocity();
            LevelManager.instance.ChangeLevel(level);

            GetComponent<Collider2D>().enabled = false;
        }
    }
}
