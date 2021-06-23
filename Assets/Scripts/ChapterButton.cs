using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChapterButton : MonoBehaviour
{
    public GameObject panel;
    private bool looking = false;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        int minLevel = panel.GetComponentInChildren<LevelButton>().LevelToLoad;
        foreach (LevelButton button in panel.GetComponentsInChildren<LevelButton>())
        {
            if (button.LevelToLoad < minLevel)
            {
                minLevel = button.LevelToLoad;
            }
        }
        GetComponent<Button>().interactable = PlayerPrefs.GetInt("Progress", -1) >= minLevel - 1;
        GetComponent<Button>().onClick.AddListener(ToggleVisibility);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleVisibility()
    {
        looking = !looking;
        panel.SetActive(looking);
        GetComponentInParent<MenuManager>().LevelSelectPanel.SetActive(!looking);
    }
}
