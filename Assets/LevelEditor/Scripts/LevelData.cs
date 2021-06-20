using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Level Editor/Level Data")]
public class LevelData : ScriptableObject
{
    public static LevelData CreateInstance() => CreateInstance<LevelData>();

    public static void Create(in LevelMappings mappings, out LevelData data)
    {
        data = CreateInstance();
        data.mappings = mappings;
    }

    public static void Import(in Texture2D rawData, in LevelMappings mappings, out LevelData data)
    {
        Create(in mappings, out data);
        for (int y = 0; y < rawData.height; y++)
        {
            for (int x = 0; x < rawData.width; y++)
            {

            }
        }
    }

    [System.Serializable]
    public struct Object
    {
        public Color color;
        public PositionData positionData;

        public Color GetObjectColor() => color;

        public bool TryGetGameObject(LevelMappings map, out GameObject obj)
        {
            try
            {
                obj = map.GetGameObject(color);
                return true;
            }
            catch (KeyNotFoundException)
            {
                obj = null;
                return false;
            }
        }
        public bool TryGetTile(LevelMappings map, out TileBase tile)
        {
            try
            {
                tile = map.GetTile(color);
                return true;
            }
            catch (KeyNotFoundException)
            {
                tile = null;
                return false;
            }
        }
        public bool TryGetAny(LevelMappings map, out GameObject obj, out TileBase tile)
        {
            if (TryGetGameObject(map, out obj))
            {
                tile = null;
                return true;
            }
            else
            {
                return TryGetTile(map, out tile);
            }
        }

        public Object(Color color, PositionData positionData)
        {
            this.color = color;
            this.positionData = positionData;
        }
    }
    [System.Serializable]
    public struct PositionData
    {
        public Vector2Int position;
        public float rotation;
        public Vector2 scale;

        public PositionData(Vector2Int position)
        {
            this.position = position;
            rotation = 0f;
            scale = Vector2.one;
        }
        public PositionData(Vector2 position) : this(position.RoundToInt()) { }
        public PositionData(Vector2Int position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
            scale = Vector2.zero;
        }
        public PositionData(Vector2 position, float rotation) : this(position.RoundToInt(), rotation) { }
        public PositionData(Vector2Int position, Vector2 scale)
        {
            this.position = position;
            rotation = 0f;
            this.scale = scale;
        }
        public PositionData(Vector2 position, Vector2 scale) : this(position.RoundToInt(), scale) { }
        public PositionData(Vector2Int position, float rotation, Vector2 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        public PositionData(Vector2 position, float rotation, Vector2 scale) : this(position.RoundToInt(), rotation, scale) { }
        
    }

    public LevelMappings Mappings => mappings;
    [SerializeField]
    private LevelMappings mappings;
    [SerializeField]
    private List<Object> data;

    public void Add(Object value)
    {
        data.Add(value);
    }
}