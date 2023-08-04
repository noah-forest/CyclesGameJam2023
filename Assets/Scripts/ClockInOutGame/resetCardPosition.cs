using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class resetCardPosition : MonoBehaviour
{
    public bool outOfBounds = false;
    public UnityEvent OutOfBounds = new UnityEvent();
    
    private void OnTriggerEnter(Collider other)
    {
        if (outOfBounds) return;
        StartCoroutine(OutOfBoundsTimer());
    }

    private void OnTriggerExit(Collider other)
    {
        outOfBounds = false;
    }

    private IEnumerator OutOfBoundsTimer()
    {
        yield return new WaitForSeconds(0.5f);
        outOfBounds = true;
        OutOfBounds.Invoke();
    }
}
