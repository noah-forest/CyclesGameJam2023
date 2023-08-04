using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioSource audioSource;
    
    public bool debugMode = false;
    
    public Minigame loadingScene;
    public Minigame startingGame;
    public Minigame[] games;

    public Animator fadeAnimator;

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

    public bool minigameEnded { private set; get; }

    public UIManager uiManager;
    public TextMeshPro gameText;

    private float _cash;
    public float cash
    {
        set
        {
            _cash = value;
            if (uiManager)
            {
                uiManager.UpdateCashUI(_cash);
            }
        }
        get => _cash;
    }
    private int _lives;
    public int lives
    {
        set
        {
            
            _lives = value;
            if (uiManager)
            {
                uiManager.UpdateLives(_lives);
            }
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
            if (uiManager)
            {
                uiManager.UpdateTimerUI(_currentTime);
            }
            
        }
        get => _currentTime;
    }
    public float maxTime;

    private void Awake()
    {
        if (debugMode)
        {
            currentTime = 1000;
        }
        lives = 3;
        cash = 0;

        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(this.gameObject);

        foreach(Minigame mg in games)
        {
            playTracker.Add(mg, 0);
            totalMaxPlays += mg.playMax;
        }
        
        if (startingGame)
        {
            LoadMinigameScene(startingGame);
        }
    }

    private void Update()
    {
        if (!minigameEnded)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                MinigameTimeout();
            };
        };
    }

    public void LoadMinigameScene(Minigame minigame)
    {
        Debug.Log("Loading " + minigame.sceneName);
        SceneManager.LoadScene(minigame.sceneName);
        currentMinigame = minigame;
        currentTime = currentMinigame.timerLength;
        maxTime = currentMinigame.timerLength;
        minigameEnded = false;
        Cursor.lockState = CursorLockMode.Confined;// unlock cursor incase it was locked by previous minigame when it ended.
        StartCoroutine(GameMusicPlayer.Instance.FindSpeaker());
    }

    public void FadeToMinigame(Minigame minigame)
    {
        StartCoroutine(FadeToMinigameCoroutine(minigame));
    }

    IEnumerator FadeToMinigameCoroutine(Minigame minigame)
    {
        fadeAnimator.SetBool("Transition", true);
        yield return new WaitForSeconds(1.5f);
        LoadMinigameScene(minigame);
        fadeAnimator.SetBool("Transition", false);
    }

    public void LoadNextMinigame()
    {
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

        FadeToMinigame(nextGame);
    }

    IEnumerator DelayFinished()
    {
        yield return new WaitForSeconds(1.5f);
        LoadNextMinigame();
    }

    /// <summary>
    /// called by minigames when they are completed.
    /// </summary>
    public void FinishMiniGame()
    {
        if (!minigameEnded)
        {
            audioSource.PlayOneShot(winSound);
            minigameEnded = true;
            SetGameText("Success!", Color.green);
            AddCash(currentMinigame.cashReward);
            LoadNextMinigame();
        }

    }
    /// <summary>
    /// called if you run out of time
    /// </summary>
    public void MinigameTimeout()
    {
        if (!minigameEnded)
        {
            audioSource.PlayOneShot(loseSound);

            minigameEnded = true;
            currentTime = 0;

            SetGameText("Failed!", Color.red);

            if (currentMinigame)
            {
                AddCash(-currentMinigame.cashPenalty); // subract pentalty
            }
            --lives;

            if (lives <= 0)
            {
                //game over
            }
            else
            {
                LoadNextMinigame();
            }
        }
    }
    
    //-- Helper Methods --//
    public void AddCash(float amt)
    {
        if (cash + amt < 0)
        {
            cash = 0;
            return;
        }
        //Debug.Log("Got cash");
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
