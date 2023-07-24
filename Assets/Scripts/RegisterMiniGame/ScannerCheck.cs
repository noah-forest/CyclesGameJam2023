using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerCheck : MonoBehaviour
{

    public LayerMask scanMask;

    private RegisterGameManager gameManager;
    
    private GameObject scanner;
    private bool hasBeenScanned;

    private void Start()
    {
        gameManager = RegisterGameManager.Instance;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.up*-1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.03f, scanMask, QueryTriggerInteraction.UseGlobal))
        {
            if (hit.transform.CompareTag("Scanner") && !hasBeenScanned)
            {
                gameManager.ScanItem();
                hasBeenScanned = true;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
    }
}
