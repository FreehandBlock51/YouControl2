using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Min(0)]
    public int LevelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().interactable = PlayerPrefs.GetInt("Progress", -1) >= LevelToLoad - 1;
        GetComponent<Button>().onClick.AddListener(() => MenuManager.LoadScene(LevelToLoad + 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
