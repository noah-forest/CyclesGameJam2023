using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollisionSounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSource;

    private bool collided = false;
    private bool played = false;
    
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!collided) return;
        PlayCollisionSound();
        collided = false;
        played = true;
        StartCoroutine(waitForSound());
    }

    private void OnCollisionEnter(Collision other)
    {
        if(played) return;
        collided = true;
    }

    private void PlayCollisionSound()
    {
        audioSource.pitch = Random.Range(90, 100) / 100f;
        audioSource.volume = 0.03f;
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }

    IEnumerator waitForSound()
    {
        yield return new WaitForSeconds(0.5f);
        played = false;
    }
}
