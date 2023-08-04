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
        if (other.tag.CompareTo("trash") == 0)
        {
            count++;
        }
    }

    private void Update()
    {
        if (count >= gm.numOfObjects)
        {
            gm.Completed();
        }
    }
}
