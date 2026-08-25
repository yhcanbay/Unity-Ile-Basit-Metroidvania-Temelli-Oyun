using UnityEngine;

public class TestSaveLoad : MonoBehaviour
{
    public ExampleData someData;

    void Start()
    {
        SaveLoadManager.instance.LoadExample(someData, "test.json");   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            someData.exampleString = "Example";
            someData.exampleInt = 5;
            SaveLoadManager.instance.SaveExample(someData, "test.json");
        }
    }
}
