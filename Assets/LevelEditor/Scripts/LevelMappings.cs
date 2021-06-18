using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
[System.Serializable]
public class LevelMappings : ScriptableObject
{
    [SerializeField]
    private List<GameObject> objects;
    [SerializeField]
    private TileData tiles;

    public GameObject GetGameObject(int objId)
    {
        return objects[objId];
    }
    public int GetObjID(GameObject obj)
    {
        return objects.IndexOf(obj);
    }
    public TileBase GetTile(int tileId)
    {
        return tiles[tileId];
    }
    public int GetTileID(TileBase tile)
    {
        return tiles.IndexOf(tile);
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