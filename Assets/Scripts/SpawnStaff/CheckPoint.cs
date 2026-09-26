using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPoint : MonoBehaviour
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
            collision.GetComponent<InteractAbility>().activeCheckpoint = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<InteractAbility>().activeCheckpoint = null;
        }
    }

    public void ActivateCheckpoint()
    {
        spriteRenderer.sprite = spriteEnabled;
        SpawnMode.spawnFromCheckpoint = true;
        Debug.Log(SpawnMode.spawnFromCheckpoint);
        //saveData
        SaveLoadManager.instance.SaveData(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
    }

}
