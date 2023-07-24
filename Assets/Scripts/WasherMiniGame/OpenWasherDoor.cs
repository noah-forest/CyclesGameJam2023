using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenWasherDoor : MonoBehaviour
{

    public Transform pivot;
    public Transform originalPivot;
    public GameObject WashingMachine;
    
    private Transform Door;

    private Camera cam;

    private Ray ray;
    private RaycastHit hit;

    private Animator buttonAnim;
    private Animator washerAnim;
    
    private int DoorLayerMask = 1 << 7;
    private int StartLayerMask = 1 << 8;

    [SerializeField] private float timeToWash = 5f;
    private float washTimer;
    
    private bool hasBeenOpened;
    private bool canStart = false;
    private bool washing = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        washerAnim = WashingMachine.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            
            // open and close washer door
            if (Physics.Raycast(ray, out hit, 100f, DoorLayerMask))
            {
                Door = hit.transform;
                if (!hasBeenOpened)
                {
                    OpenDoor(Door);
                    washing = false;
                }
                else CloseDoor(Door);
            }
            
            // start the washer
            if (Physics.Raycast(ray, out hit, 100f, StartLayerMask))
            {
                if (!hasBeenOpened && canStart)
                {
                    StartWasher(hit.transform);
                }
            }
        }

        if (washing)
        {
            washTimer += Time.deltaTime;
            // play animation of washer rattling lol
            washerAnim.ResetTrigger("PauseWashing");
            washerAnim.SetTrigger("Washing");
        }
        else
        {
            washerAnim.SetTrigger("PauseWashing");
            washerAnim.ResetTrigger("Washing");
        }

        if (washTimer >= timeToWash)
        {
            // clean the models? 
            Debug.Log("Washing Done!");
            washerAnim.SetTrigger("PauseWashing");
            washerAnim.ResetTrigger("Washing");
            ResetTimer();
            OpenDoor(Door);
        }
    }

    private void OpenDoor(Transform door)
    {
        door.SetPositionAndRotation(pivot.localPosition, pivot.localRotation);
        hasBeenOpened = true;
        canStart = false;
        washing = false;
    }

    private void CloseDoor(Transform door)
    {
        door.SetPositionAndRotation(originalPivot.localPosition, originalPivot.localRotation);
        hasBeenOpened = false;
        canStart = true;
    }
    
    private void StartWasher(Transform button)
    {
        buttonAnim = button.GetComponent<Animator>();
        buttonAnim.SetTrigger("WasherStart");
        canStart = false;
        washing = true;
    }

    private void ResetTimer()
    {
        washing = false;
        washTimer = 0;
    }
}
