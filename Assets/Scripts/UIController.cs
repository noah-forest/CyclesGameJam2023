using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image timerBar;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI daysText;
    public GameManager gameManager;

    public AudioSource timerAudio;
    public AudioClip[] timerSounds;

    private bool timerTicking;
    private bool timerLow;

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
            timerLow = false;
        }
        
        if (time <= 0 || PauseMenu.isPaused)
        {
            timerAudio.Pause();
        } else 
            timerAudio.UnPause();

        if (timerTicking || timerLow) return;
        timerAudio.clip = timerSounds[0];
        timerAudio.Play();
        timerTicking = true;
    }

    public void UpdateCashUI(float amt)
    {
        cashText.text = $"Score: {amt}";
    }


    public void UpdateLives(int numberOfLives)
    {
        livesText.text = $"Lives: {numberOfLives}";
    }

    public void UpdateDays(int numberOfDays)
    {
        daysText.text = $"Day: {numberOfDays}";
    }

    private void TimerLow()
    {
        timerText.color = Color.red;
        timerLow = true;
        
        //do audio stuff
        if (!timerTicking) return;
        timerAudio.clip = timerSounds[1];
        timerAudio.loop = true;
        timerAudio.Play();
        timerTicking = false;
    }

    private void ResetTimerColor()
    {
        timerText.color = Color.black;
    }
}
