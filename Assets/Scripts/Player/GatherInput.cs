using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;
    private InputActionMap minimapControlsMap;
    private InputActionMap activatorMap;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;
    public InputActionReference dashActionRef;
    public InputActionReference ladderActionRef;
    public InputActionReference interactActionRef;
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    private void OnEnable() { 
    
    }

    private void OnDisable()
    {
        
        playerMap.Disable();
        
    }
    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        minimapControlsMap = playerInput.actions.FindActionMap("MinimapControls");
        activatorMap = playerInput.actions.FindActionMap("Activators");
        playerMap.Enable();
        activatorMap.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalInput = moveActionRef.action.ReadValue<float>();
        verticalInput = ladderActionRef.action.ReadValue<float>();
        Debug.Log("Horizontal Input : " + horizontalInput);
    }

    public void DisablePlayerMap()
    {
        playerMap.Disable();
    }
    public void EnablePlayerMap()
    {
        playerMap.Enable();
    }
    public void EnableMinimapControlsMap()
    {
        minimapControlsMap.Enable();
    }
    public void DisableMinimapControlsMap()
    {
        minimapControlsMap.Disable();
    }
}
