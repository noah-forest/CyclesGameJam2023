
using System;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DragAndRotate : MonoBehaviour
{
   protected Camera cam;
   private float rotationSpeed = 580;
   [SerializeField] private float movementSpeed = 15f;

   protected Rigidbody rb;
   private Vector3 wantedPosition;
   
   public bool canRotate = true;

   private bool isDragging = false;
   private bool isRotating = false;

   private Vector3 startingDragPosition;
   private Vector3 targetPosition;

   float xRotation;
   float yRotation;

    [SerializeField]
    private float rotationSmoothSpeed = 0.001f;

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
         if (!canRotate) return;
         rb.angularDrag = 20;
         Cursor.lockState = CursorLockMode.Locked;
      }
      else if (isRotating && Input.GetMouseButtonUp(1))
      {
         SetStartingDragPosition();
         Cursor.lockState = CursorLockMode.None;
         isRotating = false;
         rb.angularDrag = 0.5f;
        }
        if (canRotate && isDragging && isRotating)
        {
            UpdateRotation();
        }
   }

   private void FixedUpdate()
   {
         if (canRotate && isDragging && isRotating)
         {
            //UpdateRotation();
         } else if (isDragging)
         {
            UpdatePosition();
         }
   }

   private void UpdateRotation()
   {
        isRotating = true;
        float xInput = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
        float yInput = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);





        xRotation = xInput * rotationSpeed * Time.deltaTime;
        yRotation = yInput * rotationSpeed * Time.deltaTime;


        Vector3 inputVector = new Vector3(yRotation, -xRotation, 0);
        //Vector3 smoothref = Vector3.zero;//unused 
        //inputVector = Vector3.SmoothDamp(rb.angularVelocity, inputVector, ref smoothref, rotationSmoothSpeed);


        xRotation = Mathf.Clamp(xRotation, -0.5f, 0.5f);
        yRotation = Mathf.Clamp(yRotation, -0.5f, 0.5f);

        // if (xRotation != 0)  Debug.Log($"X input: {Input.GetAxis("Mouse X")}; X rot: {xRotation}");
        //if(yRotation != 0) Debug.Log($"Y input: {Input.GetAxis("Mouse Y")}; Y rot: {yRotation}");

        rb.AddTorque(inputVector, ForceMode.VelocityChange);
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
