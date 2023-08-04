using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnMainMenuClicked()
    {
        Destroy(GameMusicPlayer.Instance.gameObject);
        Destroy(GameManager.singleton.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
