using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image timerBar;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI livesText;
    public GameManager gameManager;

    public void UpdateTimerUI(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float fraction = (time * 10) % 10;
        if (fraction > 9) fraction = 0;

        timerText.text = string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, fraction);
        timerBar.fillAmount = time / gameManager.maxTime;
        //if(time <= gameManager.maxTime / 3) TimerLow();

        if (time > 0 && time <= gameManager.maxTime / 3)
        {
            TimerLow();
        }
        else
        {
            ResetTimerColor();
        }
    }

    public void UpdateCashUI(float amt)
    {
        cashText.text = $"Score: {amt}";
    }


    public void UpdateLives(int numberOfLives)
    {
        livesText.text = $"Lives: {numberOfLives}";
    }

    private void TimerLow()
    {
        timerText.color = Color.red;
    }

    public void ResetTimerColor()
    {
        timerText.color = Color.black;
    }
}
