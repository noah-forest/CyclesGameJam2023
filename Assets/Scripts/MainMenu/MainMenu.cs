using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitClicked()
    {
        Application.Quit();
    }
}
