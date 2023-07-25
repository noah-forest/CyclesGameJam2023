using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRotateLockedAxis : DragAndRotate
{
   public Vector3 lockPosition;
   public bool lockAtAwakePosition = false;
   public bool freezeRigidbodyZ = false;

   public override void Awake()
   {
      base.Awake();
      if (lockAtAwakePosition)
      {
         lockPosition = transform.position;
      }
      if (freezeRigidbodyZ)
      {
         rb.constraints = RigidbodyConstraints.FreezePositionZ;
      }
   }

   protected override bool GetTargetPosition(out Vector3 position)
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

   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawCube(lockPosition, new Vector3(1f, 1f, 0.02f));
      Gizmos.DrawLine(lockPosition,lockPosition + Vector3.forward * 2);
   }
}
