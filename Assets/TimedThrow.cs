using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimedThrow : MonoBehaviour
{
    public float speed = 0.5f;
    public float angle = 80;
    public GameObject arrow;

    public Vector3 throwForce;

    private float currentAngle = 0;
    private float maxAngle;
    private float minAngle;

    private bool canThrow = true;
    private bool thrown = false;
    private Rigidbody rb;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    
    void Start()
    {
        ++OpenWasherDoor.singleton.numOfItems;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        maxAngle = angle / 2;
        minAngle = -angle / 2;
        Reset();
    }


    IEnumerator ResetThrown()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        canThrow = true;
    }
    void Reset()
    {
        arrow.SetActive(true);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        thrown = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        StartCoroutine(ResetThrown());
    }

    void Update()
    {
        if (!thrown)
        {
            Rotate();
        }

        if (Input.GetMouseButtonDown(0) && !thrown && canThrow)
        {
            Throw();
        }
    }

    private void Rotate()
    {
        currentAngle = currentAngle + speed * Time.deltaTime; //Mathf.Clamp(, maxAngle, minAngle);
        if (currentAngle > maxAngle+0.02f || currentAngle < minAngle-0.02f)
        {
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            speed = -speed;
        }
        transform.rotation = Quaternion.Euler(0, currentAngle, 0);
    }
    
    private void Throw()
    {
        canThrow = false;
        thrown = true;
        arrow.SetActive(false);
        rb.useGravity = true;
        rb.AddForce(transform.TransformDirection(throwForce), ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Goal")
        {
            --OpenWasherDoor.singleton.numOfItems;
            // win!!11
        }
    }

    private void OnMouseDown()
    {
        Reset();
    }
}
