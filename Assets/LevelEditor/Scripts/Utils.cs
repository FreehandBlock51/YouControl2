using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector2Int RoundToInt(this Vector2 vector2)
    {
        return new Vector2Int((int)vector2.x, (int)vector2.y);
    }
    public static LevelData.Object GetObject(this LevelMappings mappings, Color color, LevelData.PositionData positionData)
    {
        return new LevelData.Object(color, positionData);
    }
    public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
    {
        int index;
        TKey[] keys = null;
        TValue[] values = null;
        dict.Keys.CopyTo(keys, 0);
        dict.Values.CopyTo(values, 0);
        index = System.Array.IndexOf(values, value);
        return keys[index];
    }
    public static void validate(this MonoBehaviour behaviour, Object obj, string name)
    {
        if (!obj)
        {
            throw new System.ArgumentNullException(name);
        }
    }
}