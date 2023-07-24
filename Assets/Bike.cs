using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bike : MonoBehaviour
{
    [HeaderAttribute("Lower = Higher Difficulty")]
    public float difficulty = 0.5f; // Higher values increase the difficulty
    public float endProgress = 100000.0f; // Value at which the game ends
    public float animationMultiplier = 1;

    private float speed = 0.0f;
    private float progress = 0.0f;
    private bool isSpamming = false;
    private KeyCode[] sequence = new KeyCode[] { KeyCode.A, KeyCode.D };
    private int currentIndex = 0;
    private float lastButtonPressTime = 0.0f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
                speed += difficulty / (timeSinceLastPress);
            }
            progress += speed; // You can adjust the multiplier for a suitable progression pace

            lastButtonPressTime = Time.time;
        }
        
        //speed = Mathf.Clamp(speed - (Time.deltaTime * Mathf.Pow(speed/10f, 2)), 0, 100);
        speed = Mathf.Clamp(speed - (speed*Time.deltaTime), 0, 100);

        // Check if the game has reached the end
        if (progress >= endProgress)
        {
            // Game ends here, do something (e.g., show a victory screen)
            Debug.Log("Game Over - You Win!");
        }

        Debug.DrawLine(transform.position, transform.position + new Vector3(speed/10f, 0, 0));
        Debug.DrawLine(transform.position + new Vector3(0, -0.3f, 0), transform.position + new Vector3(100/10, -0.3f, 0), Color.red);
        animator.SetFloat("Speed", speed * animationMultiplier);
    }
}