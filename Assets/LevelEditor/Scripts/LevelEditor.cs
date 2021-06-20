using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    [System.Serializable]
    public class SelectableObject
    {
        [SerializeField]
        private Tile tile = null;
        [SerializeField]
        private GameObject gameObject = null;

        public System.Type GetCurrentType()
        {
            if (gameObject)
            {
                return typeof(GameObject);
            }
            else if (tile)
            {
                return typeof(Tile);
            }
            else
            {
                return null;
            }
        }
        public static implicit operator bool(SelectableObject obj) => obj.GetCurrentType() != null;

        public static implicit operator Tile(SelectableObject obj) => obj.tile;
        public static implicit operator SelectableObject(Tile tile) => new SelectableObject { tile=tile };
        public static implicit operator GameObject(SelectableObject obj) => obj.gameObject;
        public static implicit operator SelectableObject(GameObject gameObject) => new SelectableObject { gameObject=gameObject };
    }

    public Tilemap tilemap;
    [SerializeField]
    private SelectableObject selectedObject;
    public void SelectNewObject(SelectableObject newObject)
    {
        selectedObject = newObject;
    }
    public void SelectNewGameObject(GameObject newGameObject) => SelectNewObject(newGameObject);
    public void SelectNewTile(Tile newTile) => SelectNewObject(newTile);
    public RectTransform deadZone;

    public LevelSerializer serializer;

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
        validate(serializer, nameof(serializer));

        print(selectedObject.GetCurrentType());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            SelectNewObject(null);
        }
        try
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(deadZone, Input.mousePosition, Camera.current))
            {
                return;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            if (Input.GetMouseButtonUp(0) && selectedObject)
            {
                try
                {
                    print($"Mouse Position: {Input.mousePosition}");
                    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    System.Type selectedType = selectedObject.GetCurrentType();
                    print($"World Point: {worldPoint}");
                    if (selectedType == typeof(Tile))
                    {
                        Vector3Int tilePoint = tilemap.WorldToCell(worldPoint);
                        print($"Tile Point: {tilePoint}");
                        tilemap.SetTile(tilePoint, selectedObject);
                        print("Tile placed Successfully");
                    }
                    else if (selectedType == typeof(GameObject))
                    {
                        GameObject go = Instantiate((GameObject)selectedObject);
                        go.transform.position = new Vector3(Mathf.Floor(worldPoint.x) + 0.5f, Mathf.Floor(worldPoint.y) + 0.5f);
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Debug.LogException(e, this);
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                try
                {
                    print($"Mouse Position: {Input.mousePosition}");
                    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    print($"World Point: {worldPoint}");

                    Vector3Int tilePoint = tilemap.WorldToCell(worldPoint);
                    print($"Tile Point: {tilePoint}");
                    tilemap.SetTile(tilePoint, null);
                    print("Tile removed Successfully");

                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector3.one, 1f);
                    if (hit.collider)
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Debug.LogException(e, this);
                }
            }
        }
    }

    public void Export(out Texture2D output)
    {
        throw new System.NotImplementedException();
    }
}
