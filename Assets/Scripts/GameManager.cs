using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<Minigame, int> playTracker = new Dictionary<Minigame, int>();
    private int totalPlays = 0;
    private int totalMaxPlays;
    /// <summary>
    /// this goes up each time all games repeat their maximum number of times.
    /// other games can use it to scale dificulty
    /// </summary>
    public int difficulty { private set; get; } 
    
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

        foreach(Minigame mg in games)
        {
            playTracker.Add(mg, 0);
            totalMaxPlays += mg.playMax;
        }
        Debug.Log(totalMaxPlays);




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
        StopAllCoroutines();
        float extraPlayChance = Mathf.Clamp(difficulty / 10, 0, 0.5f);
        float roll = Random.Range(0, 1);
        if(roll < extraPlayChance)
        {
            --playTracker[currentMinigame];
            --totalPlays;
        }
        if (playTracker.ContainsKey(currentMinigame))
        {
            ++playTracker[currentMinigame];
            ++totalPlays;
        }
        Debug.Log(totalPlays);
        Minigame nextGame;
        if (totalPlays >= totalMaxPlays) // game cycle complete
        {
            nextGame = startingGame;
            foreach(Minigame mg in games) // reset play counts
            {
                playTracker[mg] = 0;
            }
            totalPlays = 0;
            ++difficulty; // increase difficulty each time you complete a cycle
        }
        else
        {
            do
            {
                nextGame = games[Random.Range(0, games.Length)];
            } while (playTracker[nextGame] >= nextGame.playMax); // should probably make this work off a shrinking list instead since this will get slower the less uncompleted games there are.

        }

        LoadScene(nextGame);
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
        StopAllCoroutines();
        if (gameFinished || gameFailed)
        {
            return;
        }
        gameFinished = true;
        SetGameText("Success!", Color.green);
        AddCash(currentMinigame.cashReward);
        StartCoroutine(DelayFinished());

    }
    /// <summary>
    /// called if you run out of time
    /// </summary>
    public void FailMiniGame()
    {
        StopAllCoroutines();
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
        cash += amt + difficulty * 10; // oh yeah baby
    }

    public void AddTime(float time)
    {
        if (currentTime + time < 0)
        {
            currentTime = 0;
        }
        else if (currentTime + time > maxTime)
        {
            currentTime = maxTime;
        }
        else
        {
            currentTime += time;
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
