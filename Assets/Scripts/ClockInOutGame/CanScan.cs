using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanScan : MonoBehaviour
{
    public bool canBeScanned = false;
    
    private void OnTriggerEnter(Collider other)
    {
        canBeScanned = true;
    }
}
