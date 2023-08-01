using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaitForContents : MonoBehaviour
{
    public DumpsterGameManager gm;
    
    private int count;

    private void OnTriggerEnter(Collider other)
    {
        count++;
    }

    private void Update()
    {
        if (count >= gm.numOfObjects)
        {
            gm.Completed();
        }
    }
}
