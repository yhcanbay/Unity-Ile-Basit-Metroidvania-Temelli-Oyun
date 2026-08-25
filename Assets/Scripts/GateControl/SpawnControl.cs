using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    private Transform player;
    [SerializeField] private SpawnIdentifier[] spawnPoints;
    private SpawnData spawnData = new SpawnData();
    void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
        SaveLoadManager.instance.LoadData(spawnData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
        foreach (SpawnIdentifier spawnId in spawnPoints) 
        { 
            if(spawnId.spawnKey == spawnData.spawnPointKey)
            {
                player.transform.position = spawnId.transform.position;
                break;
            }
        }
        if(spawnData.facingRight == false)
        {
            player.GetComponent<Player>().ForceFlip();
        }
    }
}
