using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenWasherDoor : MonoBehaviour
{
    public GameObject[] objects;
    
    public GameObject targetText;
    public GameObject closeDoorIndicator;
    public GameObject pressButtonIndicator;
    
    public Transform pivot;
    public Transform originalPivot;
    public GameObject WashingMachine;
    public GameObject light;

    private Material baseMaterial;
    public Material wipMaterial;

    public int numOfItems;
    public int maxItems = 5;
    
    private Transform Door;

    private Camera cam;

    private Ray ray;
    private RaycastHit hit;

    private Animator buttonAnim;
    private Animator washerAnim;

    public AudioSource doorAudio;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    
    private int DoorLayerMask = 1 << 7;
    private int StartLayerMask = 1 << 8;

    //[SerializeField] private float timeToWash = 5f;
    private float washTimer;
    
    private bool hasBeenOpened;
    private bool canStart = false;
    private bool canCloseDoor = false;
    private bool washing = false;

    public Transform door;

    // Start is called before the first frame update
    void Start()
    {
        numOfItems += GameManager.singleton.difficulty; // this is /2 rounded up, aka add an item every other round
        numOfItems = Mathf.Clamp(numOfItems, 0, maxItems);
        SpawnRandomObject();
        OpenDoor();
        cam = Camera.main;
        baseMaterial = light.GetComponent<MeshRenderer>().material;
        washerAnim = WashingMachine.GetComponent<Animator>();
    }

    void SpawnRandomObject()
    {
        GameObject obj = Instantiate(objects[Random.Range(0, objects.Length)], transform.position + new Vector3(Random.Range(-150f, 150f)/100f, 0, 0), Quaternion.identity);
        Debug.Log(transform.position);
        obj.SetActive(true);
        ThrownItem timedThrow = obj.GetComponent<ThrownItem>();
        
        timedThrow.HitGoal.AddListener(() =>
        {
            numOfItems -= 1;
            timedThrow.HitGoal.RemoveAllListeners();
            if (numOfItems > 0)
            {
                SpawnRandomObject();
            }
            else
            {
                closeDoorIndicator.SetActive(true);
                canCloseDoor = true;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.isPaused) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (targetText)
            {
                Destroy(targetText);
                targetText = null;
            }
            ray = cam.ScreenPointToRay(Input.mousePosition);
            
            // open and close washer door
            if (Physics.Raycast(ray, out hit, 100f, DoorLayerMask))
            {
                if (canCloseDoor)
                {
                    CloseDoor();
                }
            }
            
            // start the washer
            if (Physics.Raycast(ray, out hit, 100f, StartLayerMask))
            {
                if (!hasBeenOpened && canStart)
                {
                    if (numOfItems <= 0)
                    {
                        StartWasher(hit.transform);
                        GameManager.singleton.FinishMiniGame();
                    }
                    
                }
            }
        }

        if (!washing) return;
        washerAnim.ResetTrigger("PauseWashing");
        washerAnim.SetTrigger("Washing");
        light.GetComponent<MeshRenderer>().material = wipMaterial;
    }

    private void OpenDoor()
    {
        door.SetPositionAndRotation(pivot.localPosition, pivot.localRotation);
        doorAudio.PlayOneShot(doorOpen);
        hasBeenOpened = true;
        canStart = false;
        washing = false;
    }

    private void CloseDoor()
    {
        closeDoorIndicator.SetActive(false);
        doorAudio.PlayOneShot(doorClose);
        pressButtonIndicator.SetActive(true);
        door.SetPositionAndRotation(originalPivot.localPosition, originalPivot.localRotation);
        hasBeenOpened = false;
        canStart = true;
    }
    
    private void StartWasher(Transform button)
    {
        pressButtonIndicator.SetActive(false);
        buttonAnim = button.GetComponent<Animator>();
        buttonAnim.SetTrigger("WasherStart");
        canStart = false;
        washing = true;
    }
}
