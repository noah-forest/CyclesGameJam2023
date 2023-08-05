using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ThrownItem : MonoBehaviour
{
    public UnityEvent HitGoal = new UnityEvent();
    
    public float speed = 0.5f;
    public float angle = 80;
    public GameObject arrow;
    private AudioSource _audio;

    public Vector3 throwForce;

    private float currentAngle = 0;
    private float maxAngle;
    private float minAngle;

    private bool canThrow = true;
    private bool thrown = false;
    private Rigidbody rb;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool inGoal = false;
    private bool isResetting = false;
    
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        maxAngle = angle / 2;
        minAngle = -angle / 2;
        Reset();
    }


    IEnumerator ResetThrown()
    {
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        canThrow = true;
    }
    
    IEnumerator DelayedReset()
    {
        if (!isResetting)
        {
            isResetting = true;
            yield return new WaitForSeconds(2);
            if (!inGoal)
            {
                Reset();
            }
        }
    }
    public void Reset()
    {
        isResetting = false;
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
        _audio.PlayOneShot(_audio.clip);
        canThrow = false;
        thrown = true;
        arrow.SetActive(false);
        rb.useGravity = true;
        rb.AddForce(transform.TransformDirection(throwForce), ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inGoal && other.gameObject.name == "Goal")
        {
            inGoal = true;
            HitGoal.Invoke();
            //--OpenWasherDoor.singleton.numOfItems;
            // win!!11
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!inGoal)
        {
            StartCoroutine(DelayedReset());
        }
    }

    private void OnMouseDown()
    {
        //Reset();
    }
}
