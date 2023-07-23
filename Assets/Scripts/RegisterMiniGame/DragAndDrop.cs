
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DragAndDrop : MonoBehaviour
{
   private Camera cam;
   private float vectorX, vectorY;
   private float distanceFromCamera;

   private Rigidbody r;

   public bool isDragging;
   
   private void Awake()
   {
      cam = Camera.main;
      distanceFromCamera = Vector3.Distance(transform.position, cam.transform.position);
      r = GetComponent<Rigidbody>();
   }

   private void OnMouseDrag()
   {
      Vector3 pos = Input.mousePosition;
      pos.z = distanceFromCamera;
      r.constraints = RigidbodyConstraints.FreezePositionZ; 
      pos = cam.ScreenToWorldPoint(pos);
      r.velocity = (pos - transform.position) * 15;
      
      isDragging = true;
      Debug.Log(isDragging);
   }

   private void OnMouseDown()
   {
      r.velocity = Vector3.zero;
   }

   private void OnMouseUp()
   {
      isDragging = false;
      Debug.Log(isDragging);
   }
}
