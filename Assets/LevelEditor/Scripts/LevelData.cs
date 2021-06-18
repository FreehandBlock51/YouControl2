using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public class Object : ScriptableObject
    {

    }

    private LevelMappings mappings;
    public LevelMappings Mappings => mappings;

    private List<Object> data;

    public void Add(Object value)
    {
        data.Add(value);
    }
}