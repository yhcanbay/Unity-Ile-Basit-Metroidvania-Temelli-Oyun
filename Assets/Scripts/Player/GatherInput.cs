using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    public InputActionMap playerMap;
    public InputActionMap uiMap;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;
    public InputActionReference dashActionRef;
    public InputActionReference ladderActionRef;
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
        playerMap.Enable();
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
}
