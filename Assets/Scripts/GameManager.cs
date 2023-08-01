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

    public bool gameFinished { private set; get; }
    public bool gameFailed { private set; get; }

    public UIManager uiManager;
    public TextMeshPro gameText;

    private float _cash;
    public float cash
    {
        set
        {
            _cash = value;
            uiManager.UpdateCashUI(_cash);
        }
        get => _cash;
    }
    private int _lives;
    public int lives
    {
        set
        {
            
            _lives = value;
            uiManager.UpdateLives(_lives);
        }
        get => _lives;
    }
    /// <summary>
    /// counts down
    /// </summary>
    private float _currentTime;
    public float currentTime
    {
        set
        {
            _currentTime = value;
            uiManager.UpdateTimerUI(_currentTime);
            
        }
        get => _currentTime;
    }
    public float maxTime;

    private void Awake()
    {
        lives = 3;
        cash = 0;

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
        if (gameFailed || gameFinished) return;
        currentTime -= Time.deltaTime;
        if (currentTime <= 0) FailMiniGame();
    }

    public void LoadScene(Minigame minigame)
    {
        SceneManager.LoadScene(minigame.scene.name);
        currentMinigame = minigame;
        currentTime = currentMinigame.timerLength;
        maxTime = currentMinigame.timerLength;
        Cursor.lockState = CursorLockMode.Confined;// unlock cursor incase it was locked by previous minigame when it ended.
    }

    public void LoadRandomScene()
    {
        LoadScene(games[Random.Range(0, games.Length)]);
        gameFinished = false;
        gameFailed = false;
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
        if (gameFinished || gameFailed)
        {
            return;
        }
        gameFinished = true;
        SetGameText("Success!", Color.green);
        AddCash(currentMinigame.cashReward);
        StartCoroutine(DelayFinished());

    }

    public void FailMiniGame()
    {
        gameFailed = true;
        currentTime = 0;

        SetGameText("FAILURE", Color.red);

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
        Debug.Log("Got cash");
        cash += amt; // oh yeah baby
    }

    public void AddTime(float time)
    {
        if(currentTime + time < 0)
        {
            currentTime = 0;
        }
        else if (currentTime + time > maxTime)
        {
            currentTime = maxTime;
        }
    }

    public void SetGameText(string text, Color color)
    {
        if (!gameText)
        {
            GameObject textObj = GameObject.FindGameObjectWithTag("GameText");
            if (textObj) gameText = textObj.GetComponent<TextMeshPro>();
        }
        if (gameText)
        {
            gameText.text = text;
            gameText.color = color;
        }
    }
}
