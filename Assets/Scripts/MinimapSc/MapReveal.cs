using UnityEngine;

public class MapReveal : MonoBehaviour
{
    [SerializeField] private string mapKey;
    private MinimapDisplayController minimapDisplayController;

    private void Start()
    {
        minimapDisplayController = FindAnyObjectByType<MinimapDisplayController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            minimapDisplayController.loadMinimapData();
            minimapDisplayController.minimapData.AddToListWithCheck(mapKey);
            minimapDisplayController.DisplayUnlockedMinimaps();
        }    
    }
}
