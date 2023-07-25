
using System;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DragAndRotate : MonoBehaviour
{
   private Camera cam;
   [SerializeField] private float rotationSpeed = 15f;
   [SerializeField] private float movementSpeed = 15f;

   private Rigidbody rb;
   private Vector3 wantedPosition;

   public Vector3 lockPosition;
   
   private bool isDragging = false;
   private bool isRotating = false;

   private Vector3 startingDragPosition;
   private Vector3 targetPosition;
   public bool lockAtAwakePosition = false;
   public bool freezeRigidbodyZ = false;

   private void Awake()
   {
      cam = Camera.main;
      rb = GetComponent<Rigidbody>();

      if (lockAtAwakePosition)
      {
         lockPosition = transform.position;
      }

      if (freezeRigidbodyZ)
      {
         rb.constraints = RigidbodyConstraints.FreezePositionZ;
      }
   }

   private void Update()
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
         if (isDragging && isRotating)
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
      if (GetMousePositionOnPlane(out targetPosition))
      {
         rb.velocity = (targetPosition + startingDragPosition - transform.position) * movementSpeed;
      }
   }

   public bool GetMousePositionOnPlane(out Vector3 position)
   {
      Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
      Plane plane = new Plane(Vector3.forward, lockPosition);

      float rayDistance;
      if (plane.Raycast(mouseRay, out rayDistance))
      {
         position = mouseRay.GetPoint(rayDistance);
         return true;
      }

      position = Vector3.zero;
      return false;
   }

   private void SetStartingDragPosition()
   {
      GetMousePositionOnPlane(out startingDragPosition);
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

   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawCube(lockPosition, new Vector3(1f, 1f, 0.02f));
      Gizmos.DrawLine(lockPosition,lockPosition + Vector3.forward * 2);
   }
}
