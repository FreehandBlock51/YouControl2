using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName = "Level Editor/Level Serializer")]
public class LevelSerializer : ScriptableObject
{
    public static LevelSerializer FromMappings(LevelSerializer map) => FromMappings(map, map.name);
    public static LevelSerializer FromMappings(LevelSerializer map, string name)
    {
        LevelSerializer serializer = CreateInstance<LevelSerializer>();
        serializer.objectMap = map.objectMap;
        serializer.objectMap.TrimExcess();
        serializer.tileMap = map.tileMap;
        serializer.tileMap.TrimExcess();
        serializer.levelObjects = new List<LevelObject>();
        serializer.name = name;
        return serializer;
    }

    [Header("Mappings")]

    [SerializeField]
    private List<GameObject> objectMap;
    [SerializeField]
    private List<Tile> tileMap;

    public LevelObject SerializeGameObject(GameObject gameObject)
    {
        int index = FindIndex(gameObject);
        if (index >= 0)
        {
            return levelObjects[index];
        }

        return new LevelObject()
        {
            hash = gameObject.GetHashCode(),
            isTile = false,
            transform = new SerializedTransform(gameObject.transform.position, gameObject.transform.rotation.z, gameObject.transform.localScale),
            other = gameObject.GetComponent<Portal>() ? SerializeGameObject(gameObject.GetComponent<Portal>().other.gameObject) : null,
            type = objectMap.FindIndex((GameObject obj) =>
                obj == gameObject || gameObject.name.StartsWith(obj.name)
            )
        };
    }
    public int FindIndex(GameObject gameObject)
    {
        try
        {
            return levelObjects.FindIndex((LevelObject obj) => { Debug.Log(obj); return obj.hash == gameObject.GetHashCode(); });
        }
        catch (System.NullReferenceException)
        {
            return -1;
        }
    }
    public LevelObject SerializeTile(Tile tile)
    {
        int index = FindIndex(tile);
        if (index >= 0)
        {
            return levelObjects[index];
        }

        return new LevelObject
        {
            hash = tile.GetHashCode(),
            isTile = true,
            transform = new LevelData.PositionData(tile.gameObject.transform.position, 0f, Vector2.one),
            other = null,
            type = tileMap.FindIndex((Tile tile1) => tile1.sprite == tile.sprite)
        };
    }
    public int FindIndex(Tile tile)
    {
        try
        {
            return levelObjects.FindIndex((LevelObject obj) => { Debug.Log(obj); return obj.hash == tile.GetHashCode(); });
        }
        catch (System.NullReferenceException e)
        {
            return -1;
        }
    }
    public void RemoveLevelObject(LevelObject obj)
    {
        levelObjects.Remove(obj);
    }
    public void RemoveGameObject(GameObject gameObject)
    {
        levelObjects.RemoveAt(FindIndex(gameObject));
    }
    public void RemoveTile(Tile tile)
    {
        levelObjects.RemoveAt(FindIndex(tile));
    }
    public bool TryRemoveLevelObject(LevelObject obj)
    {
        try
        {
            RemoveLevelObject(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool TryRemoveGameObject(GameObject gameObject)
    {
        try
        {
            RemoveGameObject(gameObject);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool TryRemoveTile(Tile tile)
    {
        try
        {
            RemoveTile(tile);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public void AddLevelObject(LevelObject obj)
    {
        levelObjects.Add(obj);
    }
    public void AddGameObject(GameObject gameObject)
    {
        AddLevelObject(SerializeGameObject(gameObject));
    }
    public void AddTile(Tile tile)
    {
        AddLevelObject(SerializeTile(tile));
    }
    public void RemoveGameObjectAtPosition(Vector2 position)
    {
        levelObjects.RemoveAll((LevelObject obj) => obj.isTile == false && obj.transform.position == position);
    }
    public void RemoveTileAtPosition(Vector2 position)
    {
        levelObjects.RemoveAll((LevelObject obj) => obj.isTile == true && obj.transform.position == position);
    }

    public Vector2? DeserializeLevelObject(LevelObject obj, out GameObject gameObject, out Tile tile)
    {
        if (obj.isTile) // tile
        {
            gameObject = null;
            tile = tileMap[obj.type];
            return obj.transform.position;
        }
        else // game object
        {
            tile = null;
            gameObject = Instantiate(objectMap[obj.type]);
            gameObject.transform.position = obj.transform.position;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, obj.transform.rotation);
            gameObject.transform.localScale = obj.transform.scale;
            return null;
        }
    }

    [Space(20f)]

    [SerializeField]
    private List<LevelObject> levelObjects;
    public List<LevelObject> GetObjects() => levelObjects;
    public void ExportObjects(LevelObject[] levelObjects) => this.levelObjects.CopyTo(levelObjects);
    public void ImportObjects(IEnumerable<LevelObject> levelObjects) => this.levelObjects = new List<LevelObject>(levelObjects);
}