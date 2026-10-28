using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MinimapDisplayController : MonoBehaviour
{
    [SerializeField]
    private List<MinimapID> minimapIDs = new List<MinimapID>();
    public MinimapData minimapData = new MinimapData();
    private string loadPath;
    [SerializeField]
    private string firstMinimapToReveal = "L1";

    private void Start()
    {
        loadMinimapData();
        DisplayUnlockedMinimaps();
    }

    public void loadMinimapData()
    {
        loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileMinimapData);
        if (File.Exists(loadPath))
        {
            SaveLoadManager.instance.LoadData(minimapData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileMinimapData);
        }
    }

    public void DisplayUnlockedMinimaps()
    {
        if(minimapData.mapKeys.Count <=0)
        {
            foreach(MinimapID mapId in minimapIDs)
            {
                if(mapId.mapKey == firstMinimapToReveal)
                {
                    mapId.gameObject.SetActive(true);
                    minimapData.AddToListWithCheck(mapId.mapKey);
                    break;
                }
            }
        }
        else
        {
            foreach (MinimapID mapId in minimapIDs)
            {
                foreach(string mapKey in minimapData.mapKeys)
                {
                    if(mapKey == mapId.mapKey)
                    {
                        mapId.gameObject.SetActive(true);
                        minimapData.mapKeys.Add(mapId.mapKey);
                        break;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        SaveLoadManager.instance.SaveData(minimapData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileMinimapData);
    }

}
