using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHeldObject : MonoBehaviour
{
    private DragAndDrop dragScript;

    private Rigidbody r;
    
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        dragScript = GetComponent<DragAndDrop>();
        r = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!dragScript.isDragging) return;
        if (Input.GetMouseButton(1))
        {
            float xRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
            
            transform.Rotate(Vector3.down, xRotation);
            transform.Rotate(Vector3.right, yRotation);
        }
    }
}
