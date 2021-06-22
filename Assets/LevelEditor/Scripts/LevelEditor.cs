using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public static bool editing;

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
    private Button selectedButton;
    public void SelectNewObjectFromButton(SelectableObject newObject, Button button)
    {
        if (selectedButton)
        {
            selectedButton.interactable = true;
        }
        SelectNewObject(newObject);
        if (button)
        {
            button.interactable = false;
        }
        selectedButton = button;
    }
    public void SelectNewGameObject(GameObject newGameObject) => SelectNewObject(newGameObject);
    public void SelectNewTile(Tile newTile) => SelectNewObject(newTile);
    public void SelectNewGameObjectFromButton(GameObject newGameObject, Button button) => SelectNewObjectFromButton(newGameObject, button);
    public void SelectNewTileFromButton(Tile newTile, Button button) => SelectNewObjectFromButton(newTile, button);
    public RectTransform deadZone;
    [SerializeField]
    private bool canEdit;

    public LevelSerializer map;
    private LevelSerializer serializer;

    // Start is called before the first frame update
    void Start()
    {
        this.validate(tilemap, nameof(tilemap));
        this.validate(map, nameof(map));
        serializer = LevelSerializer.FromMappings(map);

        print(selectedObject.GetCurrentType());
        if (!selectedObject)
        {
            selectedButton = null;
        }
        canEdit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            SelectNewObjectFromButton(null, null);
        }
        try
        {
            canEdit = !RectTransformUtility.RectangleContainsScreenPoint(deadZone, Input.mousePosition, Camera.main);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            canEdit = false;
        }
        finally
        {
            if (canEdit)
            {
                if (Input.GetMouseButtonUp(0) && selectedObject)
                {
                    PlaceSelectedObject();
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    DeleteObjects();
                }
            }
        }
    }

    private void OnEnable()
    {
        editing = true;
    }
    private void OnDisable()
    {
        editing = false;
    }

    public void PlaceSelectedObject()
    {
        if (selectedObject == null)
        {
            return;
        }
        try
        {
            print($"Mouse Position: {Input.mousePosition}");
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            System.Type selectedType = selectedObject.GetCurrentType();
            print($"World Point: {worldPoint}");
            DeleteObjects();
            if (selectedType == typeof(Tile))
            {
                Vector3Int tilePoint = tilemap.WorldToCell(worldPoint);
                print($"Tile Point: {tilePoint}");
                tilemap.SetTile(tilePoint, selectedObject);
                serializer.AddTile(tilemap.GetTile<Tile>(tilePoint));
                print("Tile placed Successfully");
            }
            else if (selectedType == typeof(GameObject))
            {
                GameObject go = Instantiate((GameObject)selectedObject);
                go.transform.position = new Vector3(Mathf.Floor(worldPoint.x) + 0.5f, Mathf.Floor(worldPoint.y) + 0.5f);
                serializer.AddGameObject(go);
                print("Game Object placed successfully");
            }
            else
            {
                print("No objects to place!");
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogException(e, this);
        }
    }
    public void DeleteObjects()
    {
        try
        {
            print($"Mouse Position: {Input.mousePosition}");
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            print($"World Point: {worldPoint}");

            Vector3Int tilePoint = tilemap.WorldToCell(worldPoint);
            print($"Tile Point: {tilePoint}");
            serializer.RemoveTileAtPosition((Vector2Int)tilePoint);
            tilemap.SetTile(tilePoint, null);
            print("Tile removed Successfully");

            serializer.RemoveGameObjectAtPosition(new Vector2(Mathf.Floor(worldPoint.x) + 0.5f, Mathf.Floor(worldPoint.y) + 0.5f));
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector3.one, 0f);
            if (hit.collider)
            {
                Destroy(hit.collider.gameObject);
            }
            print("Game Objects removed successfully");
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogException(e, this);
        }
    }

    public void Export(out string output)
    {
        output = Export(serializer);
    }
    public static string Export(LevelSerializer serializer)
    {
        string output = Newtonsoft.Json.JsonConvert.SerializeObject(serializer.GetObjects(), new Newtonsoft.Json.JsonSerializerSettings {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        print(output);
        return output;
    }
    public void ExportToFile()
    {
        SelectNewObjectFromButton(null, null);
        ExportToFile(serializer);
    }
    public static void ExportToFile(LevelSerializer serializer)
    {
        string output = Export(serializer);

        SimpleFileBrowser.FileBrowser.SetFilters(false, new SimpleFileBrowser.FileBrowser.Filter("Level Files", ".lvl"));
        SimpleFileBrowser.FileBrowser.ShowSaveDialog((string[] paths) => System.IO.File.WriteAllText(paths[0], output),
            () => { }, SimpleFileBrowser.FileBrowser.PickMode.Files, title:"Export Level", saveButtonText:"Export");
    }

    public void Import(in string input)
    {

        Import(input, serializer, tilemap);
    }

    public static void Import(string input, LevelSerializer serializer, Tilemap tilemap)
    {
        serializer.ImportObjects(Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelObject>>(input));
        foreach (LevelObject obj in serializer.GetObjects())
        {
            Vector2? tilePos = serializer.DeserializeLevelObject(obj, out _, out Tile tile);
            if (tilePos != null)
            {
                Vector3Int pos = new Vector3Int();
                pos.x = (int)tilePos.Value.x;
                pos.y = (int)tilePos.Value.y;
                tilemap.SetTile(pos, tile);
            }
        }
    }
    public void ImportFromFile()
    {
        SelectNewObjectFromButton(null, null);
        foreach (LevelObject obj in serializer.GetObjects())
        {
            if (obj.isTile)
            {
                Vector3Int tilePos = new Vector3Int();
                tilePos.x = (int)obj.transform.position.x;
                tilePos.y = (int)obj.transform.position.y;
                tilemap.SetTile(tilePos, null);
            }
            else
            {
                foreach (RaycastHit2D hit in Physics2D.RaycastAll(obj.transform.position, Vector2.one, 0f))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        ImportFromFile(serializer, tilemap);
    }
    public static void ImportFromFile(LevelSerializer serializer, Tilemap tilemap)
    {
        SimpleFileBrowser.FileBrowser.SetFilters(false, new SimpleFileBrowser.FileBrowser.Filter("Level Files", ".lvl"));
        SimpleFileBrowser.FileBrowser.ShowLoadDialog((string[] paths) => Import(System.IO.File.ReadAllText(paths[0]), serializer, tilemap),
            () => { }, SimpleFileBrowser.FileBrowser.PickMode.Files, initialFilename:"*.lvl", title:"Import Level", loadButtonText:"Import");
    }

    public void BackToMainMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
}
