using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapActivator : MonoBehaviour
{

    [SerializeField] private InputActionReference activatorActionRef;
    [SerializeField] private CanvasGroup minimapCanvasGroup;
    private Player player;
    private bool mapActivated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        activatorActionRef.action.performed += TryToOpenMinimap;
    }
    private void OnDisable()
    {
        activatorActionRef.action.performed -= TryToOpenMinimap;
    }

    private void TryToOpenMinimap(InputAction.CallbackContext context)
    {
        if (mapActivated)
        {
            //disable
            minimapCanvasGroup.alpha = 0;
            player.gatherInput.DisableMinimapControlsMap();
            mapActivated = !mapActivated;

            if(player.playerStats.GetHealth() > 0)
            {
                player.gatherInput.EnablePlayerMap();
            }
        }
        else
        {
            //enable
            minimapCanvasGroup.alpha = 1;
            player.gatherInput.EnableMinimapControlsMap();
            mapActivated = !mapActivated;

            if (player.playerStats.GetHealth() > 0)
            {
                player.gatherInput.DisablePlayerMap();
            }
        }
    }
}
