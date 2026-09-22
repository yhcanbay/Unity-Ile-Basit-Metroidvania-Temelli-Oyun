using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChenckPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private CircleCollider2D circleCol;
    [SerializeField] private CheckpointData checkpointData;

    [Header("Action Referance")]

    [SerializeField] private InputActionReference interactActionRef;

    private void OnEnable()
    {
        interactActionRef.action.performed += TryToInteractWithCheckpoint;
    }
    private void OnDisable()
    {
        interactActionRef.action.performed -= TryToInteractWithCheckpoint;
    }

    private void TryToInteractWithCheckpoint(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.sprite = spriteEnabled;
            SpawnMode.spawnFromCheckpoint = true;
            //saveData
            SaveLoadManager.instance.LoadData(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        }
    }

}
