using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject LevelSelectPanel;
    public GameObject MainMenuPanel;

    private void Awake() => LoadMainMenu();

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

    public void LoadLevel(int buildIndex) => SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
}

