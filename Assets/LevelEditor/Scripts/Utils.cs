using UnityEngine;

public static class Utils
{
    public static Vector2Int RoundToInt(this Vector2 vector2)
    {
        return new Vector2Int((int)vector2.x, (int)vector2.y);
    }
    public static LevelData.Object GetObject(this LevelMappings mappings, int id, LevelData.PositionData positionData)
    {
        return new LevelData.Object(id, positionData);
    }
}