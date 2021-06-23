using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Level Editor/Level Mappings")]
[System.Serializable]
public class LevelMappings : ScriptableObject
{
    private Dictionary<Color, GameObject> objects;
    private Dictionary<Color, Tile> tiles;
    [System.Serializable]
    public struct MapEntry<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
    public List<MapEntry<Color, GameObject>> objectMap;
    public List<MapEntry<Color, Tile>> tileMap;

    private void Awake()
    {
        objects ??= new Dictionary<Color, GameObject>(objectMap.Count);
        tiles ??= new Dictionary<Color, Tile>(tileMap.Count);
        foreach (MapEntry<Color, GameObject> entry in objectMap)
        {
            try
            {
                objects.Add(entry.key, entry.value);
            }
            catch (System.ArgumentException)
            {
                continue;
            }
        }
        foreach (MapEntry<Color, Tile> entry in tileMap)
        {
            try
            {
                tiles.Add(entry.key, entry.value);
            }
            catch (System.ArgumentException)
            {
                continue;
            }
        }
    }

    public GameObject GetGameObject(Color objColor)
    {
        return objects[objColor];
    }
    public Color GetObjectColor(GameObject obj)
    {
        return objects.GetKey(obj);
    }
    public TileBase GetTile(Color tileColor)
    {
        return tiles[tileColor];
    }
    public Color GetTileColor(Tile tile)
    {
        return tiles.GetKey(tile);
    }

    [System.Serializable]
    public class MapItem
    {
        public MapItem(object value, System.Type type)
        {
            this.value = value;
            this.type = type;
        }
        public MapItem(object value) : this(value, value.GetType()) { }
        private System.Type type;
        [SerializeReference]
        private object value;

        public object GetValueAsType()
        {
            return System.Convert.ChangeType(value, type);
        }
        public T GetValueAsType<T>()
        {
            if (typeof(T).BaseType != type)
            {
                throw new System.ArgumentException("Type mismatch", "T");
            }
            return (T)GetValueAsType();
        }
        public void SetValueAsType(object value)
        {
            this.value = System.Convert.ChangeType(value, type);
        }
        public System.Type Type { get { if (type == null) { type = value.GetType(); } return type; } set => type = value; }
    }
    public class MapItem<T> : MapItem
    {
        public MapItem(T value) : base(value, typeof(T)) { }

        public T GetValue() => GetValueAsType<T>();
    }

    [System.Serializable]
    public class TileData
    {
        [SerializeField]
        private GameObject mapObject;
        private Tilemap tilemap => mapObject.GetComponentInChildren<Tilemap>();
        [SerializeField]
        private BoundsInt bounds;

        public TileBase[] Tiles => tilemap.GetTilesBlock(bounds);

        public TileBase this[int index] => Tiles[index];
        public int IndexOf(TileBase tile) => System.Array.IndexOf(Tiles, tile);
    }
}