using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stinker : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float minWalkDuration = 1f;
    public float maxWalkDuration = 3f;
    public Animator animator;

    public GameObject chad;

    private bool isWalking = false;
    private Vector2 currentDirection = Vector2.zero;
    private float walkTimer = 0f;
    private float notWalkingTimer = 0f;
    private float panickedTimer = 0f;
    private Rigidbody2D rb;
    private bool isPanicked = false;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("Walking", isWalking);
        RandomizeWalk();
    }

    private void FixedUpdate()
    {
        // Update Animator parameters
        animator.SetBool("Walking", isWalking);
        FlipSprite();

        if (isWalking)
        {
            // Continue walking in the current direction
            rb.velocity = currentDirection * walkSpeed;

            // Decrease the walkTimer and check if it's time to stop walking
            walkTimer -= Time.deltaTime;
            if (walkTimer <= 0f)
            {
                isWalking = false;
                rb.velocity = Vector2.zero;
                notWalkingTimer = Random.Range(0.5f, 1.5f);
            }
        }
        else
        {
            // Continue not walking for the specified duration
            notWalkingTimer -= Time.deltaTime;
            if (notWalkingTimer <= 0f)
            {
                RandomizeWalk();
            }
        }

        if (isPanicked)
        {
            if (Random.Range(1, 30) == 1)
            {
                RandomizeWalk();
            }
            rb.velocity = currentDirection * walkSpeed * 3.5f;
            panickedTimer -= Time.deltaTime;
            if (panickedTimer <= 0)
            {
                isPanicked = false;
            }
            animator.SetBool("Panicked", isPanicked);
        }
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(currentDirection.x, currentDirection.y) * 5);
    }

    private void RandomizeWalk()
    {
        // Generate a random walk duration
        walkTimer = Random.Range(minWalkDuration, maxWalkDuration);

        // Randomly choose a new direction
        currentDirection = new Vector2(Random.Range(-100, 100) / 100f, Random.Range(-100, 100) / 100f).normalized;

        // Start walking
        isWalking = true;
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            Vector3 scale = transform.localScale;
            if (rb.velocity.x > 0)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Randomly choose a new direction
        currentDirection = new Vector2(Random.Range(-100, 100) / 100f, Random.Range(-100, 100) / 100f).normalized;

        // Start walking again
        isWalking = true;
        walkTimer = Random.Range(minWalkDuration, maxWalkDuration);
    }

    public void Spray()
    {
        _audioSource.Play();
        panickedTimer = 2f;
        isPanicked = true;

        ParticleSystem[] stinkParticle;
        stinkParticle = GetComponentsInChildren<ParticleSystem>();
        if (stinkParticle.Length > 0)
        {
            Destroy(stinkParticle[0].gameObject);
        }
        
        if (stinkParticle.Length - 1 == 0)
        {
            Instantiate(chad, transform.position, chad.transform.rotation);
            Destroy(this.gameObject);
        }

    }
}
