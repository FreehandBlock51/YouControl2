using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Level Editor/Level Serializer")]
public class LevelSerializer : ScriptableObject
{
    public static LevelSerializer FromMappings(LevelSerializer map)
    {
        map.levelObjects = new List<LevelObject>();
        return map;
    }

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

        return new LevelObject {
            hash = gameObject.GetHashCode(),
            isTile = false,
            transform = new LevelData.PositionData(gameObject.transform.position, gameObject.transform.rotation.z, gameObject.transform.localScale),
            other = gameObject.GetComponent<Portal>() ? SerializeGameObject(gameObject.GetComponent<Portal>().other.gameObject) : null,
            type = objectMap.FindIndex((GameObject obj) => obj == gameObject || obj.GetComponents<MechanicBehaviour>() == gameObject.GetComponents<MechanicBehaviour>())
        };
    }
    public int FindIndex(GameObject gameObject)
    {
        return levelObjects.FindIndex((LevelObject obj) => obj.hash == gameObject.GetHashCode());
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
        return levelObjects.FindIndex((LevelObject obj) => obj.hash == tile.GetHashCode());
    }
    public void RemoveGameObject(GameObject gameObject)
    {
        levelObjects.RemoveAt(FindIndex(gameObject));
    }
    public void RemoveTile(Tile tile)
    {
        levelObjects.RemoveAt(FindIndex(tile));
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

    [SerializeField]
    private List<LevelObject> levelObjects;
    public List<LevelObject> GetObjects() => levelObjects;
    public void ExportObjects(LevelObject[] levelObjects) => this.levelObjects.CopyTo(levelObjects);
    public void ImportObjects(IEnumerable<LevelObject> levelObjects) => this.levelObjects = new List<LevelObject>(levelObjects);
}