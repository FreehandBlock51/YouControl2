using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap tilemap;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Export(System.IO.Stream output)
    {
        Debug.Assert(output.CanWrite);
    }
}
