using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public SaveLoadManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveExample(ExampleData data,string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }
}
