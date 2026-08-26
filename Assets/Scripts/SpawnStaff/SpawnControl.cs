using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    private Transform player;
    [SerializeField] private SpawnIdentifier[] spawnPoints;
    [SerializeField] private SpawnIdentifier[] spawnCheckPoints;
    private SpawnData spawnData = new SpawnData();
    private CheckpointData checkPointData = new CheckpointData();
    void Start()
    {
        player = FindAnyObjectByType<Player>().transform;

        if(SpawnMode.spawnFromCheckpoint == true)
        {
            SaveLoadManager.instance.LoadData(checkPointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            foreach (SpawnIdentifier spawnId in spawnCheckPoints)
            {
                if (spawnId.spawnKey == checkPointData.checkPointKey)
                {
                    player.transform.position = spawnId.transform.position;
                    break;
                }
            }
            if (checkPointData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
            SpawnMode.spawnFromCheckpoint = false;
        }
        else
        {
            SaveLoadManager.instance.LoadData(spawnData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
            foreach (SpawnIdentifier spawnId in spawnPoints)
            {
                if (spawnId.spawnKey == spawnData.spawnPointKey)
                {
                    player.transform.position = spawnId.transform.position;
                    break;
                }
            }
            if (spawnData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
        }
    }
}
