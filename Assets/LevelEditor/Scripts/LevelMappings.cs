using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class LevelMappings : ScriptableObject
{
    public List<GameObject> objects;

    public GameObject Convert(int id)
    {
        return objects[id];
    }
    public int Convert(GameObject obj)
    {
        return objects.IndexOf(obj);
    }
}