using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerCheck : MonoBehaviour
{

    public LayerMask scanMask;
    
    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.02f, scanMask, QueryTriggerInteraction.UseGlobal))
        {
            Debug.Log("hit scanner");
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
    }
}
