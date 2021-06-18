using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
[JsonObject(MemberSerialization.Fields)]
public struct LevelObject
{
    [JsonRequired]
    public int id;
    public LevelData.PositionData transform;
    public bool isTile;
    public int otherId;
}