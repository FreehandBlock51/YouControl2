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
    public SerializedTransform transform;
    public bool isTile;
    public LevelObject other;

    public LevelObject()
    {
        
    }

    public override string ToString()
    {
        return base.ToString() + $" {hash}";
    }
}

[JsonObject(MemberSerialization.Fields)]
public class SerializedTransform
{
    public SerializedVector2 position;
    public float rotation;
    public SerializedVector2 scale;

    public SerializedTransform() 
    {
        position = Vector2.zero;
        rotation = 0f;
        scale = Vector2.one;
    }
    public SerializedTransform(SerializedVector2 position, float rotation, SerializedVector2 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public override string ToString()
    {
        return $"{position} {rotation} {scale} ({GetType().Name})";
    }

    public static implicit operator SerializedTransform(LevelData.PositionData data) => new SerializedTransform { position = (Vector2)data.position, rotation = data.rotation, scale = data.scale };
}

[JsonObject(MemberSerialization.Fields)]
public struct SerializedVector2
{
    public float x;
    public float y;

    public SerializedVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public static implicit operator SerializedVector2(Vector2 vector) => new SerializedVector2(vector.x, vector.y);
    public static implicit operator Vector2(SerializedVector2 vector) => new Vector2(vector.x, vector.y);
    public static implicit operator SerializedVector2(Vector3 vector) => (Vector2)vector;
    public static implicit operator Vector3(SerializedVector2 vector) => new Vector2(vector.x, vector.y);
}