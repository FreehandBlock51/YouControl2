using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap tilemap;
    public UnityEngine.Tilemaps.Tile selectedTile;

    public LevelData data;
    public LevelMappings mappings;

    // Start is called before the first frame update
    void Start()
    {
        void validate(Object obj, string name)
        {
            if (!obj)
            {
                throw new System.ArgumentNullException(name);
            }
        }
        validate(tilemap, nameof(tilemap));
        validate(mappings, nameof(mappings));

        LevelData.Create(in mappings, out data);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            print("e");
            tilemap.SetTile(tilemap.WorldToCell(Camera.current.ScreenToWorldPoint(Input.mousePosition)), selectedTile);
        }
    }

    public void Export(out Texture2D output)
    {
        throw new System.NotImplementedException();
    }
}
