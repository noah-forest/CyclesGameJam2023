using System.Collections;
using System.Threading;
using TMPro;
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

    public UIManager uiManager;
    public TextMeshPro gameText;

    private float _cash;
    public float cash
    {
        set
        {
            uiManager.UpdateCashUI(value);
            _cash = value;
        }
        get => _cash;
    }
    private int _lives;
    public int lives
    {
        set
        {
            uiManager.UpdateLives(value);
            _lives = value;
        }
        get => _lives;
    }

    public float _currentTime;
    public float currentTime
    {
        set
        {
            uiManager.UpdateTimerUI(value);
            _currentTime = value;
        }
        get => _currentTime;
    }
    public float maxTime;


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

    private void Update()
    {
        currentTime -= Time.deltaTime;
    }

    public void LoadScene(Minigame minigame)
    {
        SceneManager.LoadScene(minigame.scene.name);
        currentMinigame = minigame;
        maxTime = currentMinigame.timerLength;
        GameObject textObj = GameObject.FindGameObjectWithTag("GameText");
        if(textObj) gameText = textObj.GetComponent<TextMeshPro>();


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

    /// <summary>
    /// called by minigames when they are completed.
    /// </summary>
    public void FinishMiniGame()
    {
        if (gameFinished)
        {
            return;
        }
        gameFinished = true;
        StartCoroutine(DelayFinished());
        AddCash(currentMinigame.cashReward);
    }

    public void FailMiniGame()
    {
        if (gameText)
        {
            gameText.text = "FAILURE";
        }

        AddCash(-currentMinigame.cashPenalty); // subract pentalty
        --lives;

        if (lives <= 0)
        {
            //game over

        }
        else
        {
            StartCoroutine(DelayFinished());
        }
    }


    public void AddCash(float amt)
    {
        if (cash + amt < 0)
        {
            cash = 0;
            return;
        }
        cash += amt; // oh yeah baby
    }
}
