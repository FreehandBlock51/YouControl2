using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject LevelSelectPanel;
    public GameObject MainMenuPanel;

    private void Start()
    {
        LoadMainMenu();
        foreach (ChapterButton button in GetComponentsInChildren<ChapterButton>(true))
        {
            button.panel.SetActive(false);
        }
    }

    public void LoadLevelSelect()
    {
        LevelSelectPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        LevelSelectPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void LoadLevel(int buildIndex) => LoadScene(buildIndex);
    public void QuitLevel() => Application.Quit(0);

    public static void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex + 2, LoadSceneMode.Single);
    }

    public void PlayUserLevel() => PlayLevelFile();
    public static void PlayLevelFile(string path = default)
    {
        if (string.IsNullOrEmpty(path))
        {
            SimpleFileBrowser.FileBrowser.SetFilters(false, new SimpleFileBrowser.FileBrowser.Filter("Level Files", ".lvl"));
            SimpleFileBrowser.FileBrowser.ShowLoadDialog((paths) => PlayLevelFile(paths[0]), () => { }, SimpleFileBrowser.FileBrowser.PickMode.Files, title: "Import Level", loadButtonText: "Import");
        }
        else
        {
            LevelPlayer.FilePath = path;
            LoadScene(0);
        }
    }
    public static void PlayLevel(string raw)
    {
        LevelPlayer.RawData = raw;
        LoadScene(0);
    }
}

