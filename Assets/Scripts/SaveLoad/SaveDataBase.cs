using System;
using UnityEngine;

[Serializable]
public class ExampleData
{
    public int exampleInt;
    public string exampleString;
}
[Serializable]
public class SpawnData
{
    public string spawnPointKey;
    public bool facingRight;

    public SpawnData()
    {
        spawnPointKey = "Start";
        facingRight = true;
    }
}
