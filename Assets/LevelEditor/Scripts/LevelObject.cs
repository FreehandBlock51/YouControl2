using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
[JsonObject(MemberSerialization.Fields)]
public class LevelObject
{
    public int hash;
    [JsonRequired]
    public int type;
    public LevelData.PositionData transform;
    public bool isTile;
    public LevelObject other;

    public LevelObject()
    {
        
    }
}