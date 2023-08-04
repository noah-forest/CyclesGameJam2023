using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollisionSounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSource;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        float speed = rb.velocity.magnitude;
        audioSource.pitch = Random.Range(90, 120) / 100f;
        audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
