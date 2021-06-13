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

    public void LoadLevel(int buildIndex) => LoadScene(buildIndex);
    public void QuitLevel() => Application.Quit(0);

    public static void LoadScene(int buildIndex)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(scene);
    }
}

