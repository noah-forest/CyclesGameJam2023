using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bike : MonoBehaviour
{
    [HeaderAttribute("Lower = Higher Difficulty")]
    public float difficulty = 1f; // Lower values increase the difficulty
    public float maxDiffuclty = 4;
    public float minDifficulty = 2f;
    public float endProgress = 100000.0f; // Value at which the game ends
    public float animationMultiplier = 1;
    public float maxSpeed = 100;

    private float speed = 0.0f;
    private float progress = 0.0f;
    private bool isSpamming = false;
    private KeyCode[] sequence = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1 };
    private int currentIndex = 0;
    private float lastButtonPressTime = 0.0f;

    private Animator animator;
    public RectTransform effortPanel;
    public RectTransform effortBar;
    private Image effortBarImage;

    public RectTransform progressPanel;
    public RectTransform progressBar;

    private void Start()
    {
        effortBarImage = effortBar.GetComponent<Image>();
        animator = GetComponent<Animator>();
        float diffMod = 1/(GameManager.singleton.difficulty + 1);
        difficulty = Random.Range(minDifficulty + diffMod, maxDiffuclty);
    }

    private void Update()
    {
        // Check if player has started spamming the buttons
        if (!isSpamming && Input.GetKeyDown(sequence[0]))
        {
            isSpamming = true;
            lastButtonPressTime = Time.time;
        }

        // Check if player is spamming and has pressed the correct button in the sequence
        if (isSpamming && Input.GetKeyDown(sequence[currentIndex]))
        {
            currentIndex++;
            if (currentIndex >= sequence.Length)
            {
                // Player finished the sequence, increase progress and reset
                currentIndex = 0;
            }
            float timeSinceLastPress = Time.time - lastButtonPressTime;
            if (timeSinceLastPress != 0)
            {
                speed += difficulty / (Mathf.Clamp(timeSinceLastPress, 0.02f, 9999f));
            }

            progress += speed/maxSpeed; // You can adjust the multiplier for a suitable progression pace

            lastButtonPressTime = Time.time;
        }
        
        //speed = Mathf.Clamp(speed - (Time.deltaTime * Mathf.Pow(speed/10f, 2)), 0, 100);
        speed = Mathf.Clamp(speed - (speed*Time.deltaTime), 0, maxSpeed);

        // Check if the game has reached the end
        if (progress >= endProgress)
        {
            progress = Mathf.Clamp(progress, 0, endProgress);
            GameManager.singleton.FinishMiniGame();
        }

        //Debug.DrawLine(transform.position, transform.position + new Vector3(speed/10f, 0, 0));
        //Debug.DrawLine(transform.position + new Vector3(0, -0.3f, 0), transform.position + new Vector3(100/10, -0.3f, 0), Color.red);
        animator.SetFloat("Speed", speed * animationMultiplier);
        UpdateEffortBar();
    }

    private void UpdateEffortBar()
    {
        float speedPercent = speed / maxSpeed;
        Vector2 sizeDelta = effortBar.sizeDelta;
        sizeDelta.x = effortPanel.rect.width * speedPercent;
        effortBar.sizeDelta = sizeDelta;
        effortBarImage.color = Color.Lerp(Color.green, Color.red, speedPercent);
        
        float progressPercent = progress / endProgress;
        sizeDelta = progressBar.sizeDelta;
        sizeDelta.x = progressPanel.rect.width * progressPercent;
        progressBar.sizeDelta = sizeDelta;
    }
}