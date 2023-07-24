using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenWasherDoor : MonoBehaviour
{

    public Transform pivot;
    public Transform originalPivot;
    
    private Camera cam;

    private Ray ray;
    private RaycastHit hit;

    private int layerMask = 1 << 7;

    private bool hasBeenOpened;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hasBeenOpened)
                {
                    hit.transform.SetPositionAndRotation(originalPivot.localPosition, originalPivot.localRotation);
                    hasBeenOpened = false;
                }
                else
                {
                    hit.transform.SetPositionAndRotation(pivot.localPosition, pivot.localRotation);
                    hasBeenOpened = true;
                }
            }
        }
    }
}
