using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelPlayer : MonoBehaviour
{
    public static string FilePath { get; internal set; }
    private static string _raw;
    public static string RawData
    {
        get
        {
            if (File.Exists(FilePath))
            {
                _raw = "";
                return File.ReadAllText(FilePath);
            }
            else
            {
                return _raw;
            }
        }
        internal set
        {
            _raw = value;
            FilePath = null;
        }
    }

    [SerializeField]
    private LevelSerializer serializer;
    [SerializeField]
    private Tilemap tilemap;
    
    // Use this for initialization
    void Start()
    {
        this.validate(serializer, nameof(serializer));
        this.validate(tilemap, nameof(tilemap));
        LevelEditor.Import(RawData, serializer, tilemap);
    }

    // Update is called once per frame
    void Update()
    {

    }
}