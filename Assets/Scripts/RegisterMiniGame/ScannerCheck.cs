using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScannerCheck : MonoBehaviour
{

    public LayerMask scanMask;

    private RegisterGameManager gameManager;
    
    private GameObject scanner;
    private bool hasBeenScanned;

    public float rayDistance = 0.05f;

    private void Start()
    {
        gameManager = RegisterGameManager.Instance;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, scanMask))
        {
            if (hit.transform.CompareTag("Scanner") && !hasBeenScanned)
            {
                gameManager.ScanItem();
                hasBeenScanned = true;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position - transform.up * rayDistance);
    }
}
