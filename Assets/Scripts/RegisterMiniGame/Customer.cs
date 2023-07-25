using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    public int numOfItems;
    public Animator animator;
    public float moveSpeed = 5f;
    
    private Vector3 destination;
    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            // Calculate the distance between the current position and the destination
            float distance = Vector3.Distance(transform.position, destination);

            // Move towards the destination using Lerp based on moveSpeed
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            // Check if the object is close enough to the destination (you can set a small threshold as per your requirement)
            if (distance < 0.04f)
            {
                animator.SetBool("MovingLeft", false);
                transform.position = destination;
                isMoving = false;
            }
        }
    }

    public void MoveTo(Vector3 target)
    {
        destination = target;
        animator.SetBool("MovingLeft", true);
        isMoving = true;
    }

    public void IsBeingServed(bool served)
    {
        animator.SetBool("BeingServed", served);
    }

    public void FinishedBeingServed()
    {
        animator.SetBool("Finished", true);
        Destroy(gameObject, 0.2f);
    }
}
