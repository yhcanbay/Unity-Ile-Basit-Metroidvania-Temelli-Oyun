using UnityEngine;

public class ChenckPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private CircleCollider2D circleCol;
    [SerializeField] private CheckpointData checkpointData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.sprite = spriteEnabled;
            //saveData
            SaveLoadManager.instance.LoadData(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        }
    }

}
