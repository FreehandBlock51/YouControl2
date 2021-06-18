using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public static LevelData CreateInstance() => CreateInstance<LevelData>();

    public static void Create(in LevelMappings mappings, out LevelData data)
    {
        data = CreateInstance();
        data.mappings = mappings;
    }

    public static void Import(in string rawData, in LevelMappings mappings, out LevelData data)
    {
        Create(in mappings, out data);
        throw new System.NotImplementedException();
    }

    [System.Serializable]
    public struct Object
    {
        public int id;
        public PositionData positionData;

        public int GetObjectID() => id;

        public Object(int id, PositionData positionData)
        {
            this.id = id;
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