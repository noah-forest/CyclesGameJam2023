using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject checkMenu;
    public static bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("clicked");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("clicked");
        Application.Quit();
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        //Destroy(pauseMenu.gameObject);
        Destroy(GameMusicPlayer.Instance.gameObject);
        Destroy(GameManager.singleton.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenCheckMenu()
    {
        Debug.Log("clicked");
        checkMenu.SetActive(true);
    }

    public void CloseCheckMenu()
    {
        checkMenu.SetActive(false);
    }
}
