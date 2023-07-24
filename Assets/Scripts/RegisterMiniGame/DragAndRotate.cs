
using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DragAndRotate : MonoBehaviour
{
   private Camera cam;
   [SerializeField] private float rotationSpeed = 15f;
   [SerializeField] private float movementSpeed = 15f;

   private Rigidbody r;

   private bool isDragging = false;
   private bool isRotating = false;

   private short framePassed = -1;
   
   private void Awake()
   {
      cam = Camera.main;
      r = GetComponent<Rigidbody>();
   }

   private void Update()
   {
      if (isDragging && Input.GetMouseButtonDown(1))
      {
         isRotating = true;
      }
      else if (isRotating && Input.GetMouseButtonUp(1))
      {
         isRotating = false;
      }

      if (framePassed < 2)
      {
         framePassed++;
      } else if (framePassed == 2)
      {
         framePassed = 3;
      }
   }

   private void FixedUpdate()
   {
      if (framePassed == 3)
      {
         if (isDragging && isRotating)
         {
            isRotating = true;
            float xRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
         
            r.angularVelocity = new Vector3(yRotation, -xRotation, 0);
            r.velocity = new Vector3(0, 0, 0);
         } else if (isDragging)
         {
            float x = Input.GetAxis("Mouse X") * movementSpeed;
            float y = Input.GetAxis("Mouse Y") * movementSpeed;

            r.velocity = new Vector3(x, y, 0);
         }
      }
   }

   private void OnMouseDown()
   {
      r.useGravity = false;
      r.constraints = RigidbodyConstraints.FreezePositionZ;
      isDragging = true;
      r.velocity = Vector3.zero;
      Cursor.lockState = CursorLockMode.Locked;
      framePassed = 0;
   }

   private void OnMouseUp()
   {
      r.useGravity = true;
      isDragging = false;
      Cursor.lockState = CursorLockMode.None;
   }
}
