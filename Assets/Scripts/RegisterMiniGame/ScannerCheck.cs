using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerCheck : MonoBehaviour
{

    public LayerMask scanMask;
    
    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.up*-1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.03f, scanMask, QueryTriggerInteraction.UseGlobal))
        {
            Debug.Log("hit scanner");
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
    }
}
