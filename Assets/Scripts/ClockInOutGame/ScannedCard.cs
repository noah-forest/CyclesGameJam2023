using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScannedCard : MonoBehaviour
{
    public bool cardScanned = false;
    public UnityEvent OnScan = new UnityEvent();
    
    private void OnTriggerEnter(Collider other)
    {
        cardScanned = true;
        OnScan.Invoke();
    }

}
