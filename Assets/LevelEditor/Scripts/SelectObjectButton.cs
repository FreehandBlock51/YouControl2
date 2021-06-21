using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectObjectButton : MonoBehaviour
{
    public LevelEditor editor;
    public LevelEditor.SelectableObject newObject;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => editor.SelectNewObjectFromButton(newObject, GetComponent<Button>()));
    }
}