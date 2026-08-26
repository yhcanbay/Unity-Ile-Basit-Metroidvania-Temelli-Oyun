using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    [Header("Spawn")]
    public string folderName = "SaveFiles";
    public string fileName = "SpawnPoint.json";

    [Header("CheckPoint")]
    public string fileCheckPoint = "CheckPoint.json";

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

    public void SaveData<T>(T data,string folderName, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath,folderName ,fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }

    public void LoadData<T>(T data, string folderName, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath,folderName, fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, data);
        }
    }

    public void DeleteData(string folderName, string fileName)
    {
        string deletePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
        }
    }

    public void DeleteFolder(string folderName)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath);
        }
    }

    public void SaveExample(ExampleData data,string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }

    public void LoadExample(ExampleData data, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, fileName);
        if(File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, data);
        }
    }
    public void DeleteExample(ExampleData data, string fileName) 
    { 
        string deletePath = Path.Combine(Application.persistentDataPath,fileName);
        if(File.Exists(deletePath))
        {
            File.Delete(deletePath);
        }
    }
}
