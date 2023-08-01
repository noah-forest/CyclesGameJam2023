using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI livesText;



    public void UpdateTimerUI(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float fraction = (time * 10) % 10;
        if (fraction > 9) fraction = 0;

        timerText.text = string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, fraction);
    }

    public void UpdateCashUI(float amt)
    {
        cashText.text = $"$: {amt}";
    }


    public void UpdateLives(int numberOfLives)
    {
        livesText.text = $"Lives: {numberOfLives}";
    }
}
