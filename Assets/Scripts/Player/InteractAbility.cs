using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractAbility : MonoBehaviour
{
    [SerializeField] private InputActionReference interactActionRef;

    public CheckPoint activeCheckpoint = null;

    private void OnEnable()
    {
        interactActionRef.action.performed += TryInteract;
    }

    private void OnDisable()
    {
        interactActionRef.action.performed -= TryInteract;
    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        if (activeCheckpoint != null)
        {
            activeCheckpoint.ActivateCheckpoint();
        }
    }
}
