
using System;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DragAndRotate : MonoBehaviour
{
   protected Camera cam;
   [SerializeField] private float rotationSpeed = 15f;
   [SerializeField] private float movementSpeed = 15f;

   protected Rigidbody rb;
   private Vector3 wantedPosition;
   
   public bool canRotate = true;

   private bool isDragging = false;
   private bool isRotating = false;

   private Vector3 startingDragPosition;
   private Vector3 targetPosition;

   public virtual void Awake()
   {
      cam = Camera.main;
      rb = GetComponent<Rigidbody>();
   }

   protected void Update()
   {
      if (isDragging && Input.GetMouseButtonDown(1))
      {
         isRotating = true;
         Cursor.lockState = CursorLockMode.Locked;
      }
      else if (isRotating && Input.GetMouseButtonUp(1))
      {
         SetStartingDragPosition();
         Cursor.lockState = CursorLockMode.None;
         isRotating = false;
      }
   }

   private void FixedUpdate()
   {
         if (canRotate && isDragging && isRotating)
         {
            UpdateRotation();
         } else if (isDragging)
         {
            UpdatePosition();
         }
   }

   private void UpdateRotation()
   {
      isRotating = true;
      float xRotation = Input.GetAxis("Mouse X") * rotationSpeed;
      float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
         
      rb.angularVelocity = new Vector3(yRotation, -xRotation, 0);
      rb.velocity = (targetPosition + startingDragPosition - transform.position) * movementSpeed;
   }

   private void UpdatePosition()
   {
      if (GetTargetPosition(out targetPosition))
      {
         rb.velocity = (targetPosition + startingDragPosition - transform.position) * movementSpeed;
      }
   }

   protected virtual bool GetTargetPosition(out Vector3 position)
   {
      RaycastHit hit;
      Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(mouseRay, out hit))
      {
         position = hit.point;
         return true;
      }

      position = Vector3.zero;
      return false;
   }

   private void SetStartingDragPosition()
   {
      GetTargetPosition(out startingDragPosition);
      startingDragPosition = rb.position - startingDragPosition;
   }

   private void OnMouseDown()
   {
      SetStartingDragPosition();
      rb.useGravity = false;
      isDragging = true;
   }

   private void OnMouseUp()
   {
      rb.useGravity = true;
      isDragging = false;
   }
}
