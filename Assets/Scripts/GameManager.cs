using System.Collections;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    public Minigame loadingScene;
    public Minigame startingGame;
    public Minigame[] games;
    
    public static GameManager singleton;
    public static Minigame currentMinigame = null;

    private bool gameFinished = false;

    public float cash;
    public int lives;
    public int time;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(this.gameObject);
        LoadScene(startingGame);
    }

    public void LoadScene(Minigame minigame)
    {
        SceneManager.LoadScene(minigame.scene.name);
        currentMinigame = minigame;
    }

    public void LoadRandomScene()
    {
        LoadScene(games[Random.Range(0, games.Length)]);
        gameFinished = false;
    }

    IEnumerator DelayFinished()
    {
        yield return new WaitForSeconds(1.5f);
        LoadRandomScene();
        /*if (currentScene == loadingScene)
        {
            LoadRandomScene();
        }
        else
        {
            LoadScene(loadingScene);
        }*/
    }

    public void FinishMiniGame()
    {
        if (gameFinished)
        {
            return;
        }
        gameFinished = true;
        StartCoroutine(DelayFinished());
    }
}
