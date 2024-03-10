using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject OpeningMenu;
    public GameObject DifficultyMenu;
    private AsyncOperation loadLevel;

    IEnumerator LoadLevelSceneAsync(string sceneName)
    {
        loadLevel = SceneManager.LoadSceneAsync("level");
        loadLevel.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!loadLevel.isDone)
        {
            if (loadLevel.progress >= 0.9f)
            {

                // Activate the scene when ready
                loadLevel.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void StartGame(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.Save();
        StartCoroutine(LoadLevelSceneAsync("level"));
    }

    public void OpenDifficultyMenu()
    {
        OpeningMenu.SetActive(false);
        DifficultyMenu.SetActive(true);
    }

    public void CloseDifficultyMenu()
    {
        OpeningMenu.SetActive(true);
        DifficultyMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
