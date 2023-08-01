using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RegisterGameManager : MonoBehaviour
{
    private static RegisterGameManager _instance;
    public static RegisterGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<RegisterGameManager>();
            }
            return _instance;
        }
    }

    public List<Customer> Customers = new List<Customer>();
    public List<GameObject> SpeechBubbles = new List<GameObject>();
    public List<GameObject> GroceryItems = new List<GameObject>();
    public List<GameObject> LinePositions = new List<GameObject>();
    //
    private List<Customer> lineOfCustomers = new List<Customer>();

    private int numOfCustomers;
    private int listIndex;
    private int itemIndex;
    private Customer curCustomer;
    private int speechIndex;
    private AudioSource beep;
    public Bounds spawnBounds;

    public TextMeshPro scanText;
    private int recentScanCount = 0;

    private bool willSpeak;
    
    private void Start()
    {
        beep = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        _instance = this;
        CreateCustomersInLine();
        curCustomer = lineOfCustomers[0];
        curCustomer.IsBeingServed(true);
        PickRandomSpeechBubble();
        SpawnItems();
    }

    private void CreateCustomersInLine()
    {
        numOfCustomers = Random.Range(1, LinePositions.Count+1);
        for (int i = 0; i < numOfCustomers; i++)
        {
            listIndex = Random.Range(0, Customers.Count);
            Customer cus = Instantiate(Customers[listIndex], LinePositions[i].transform.position, LinePositions[i].transform.rotation);
            cus.MoveTo(LinePositions[i].transform.position);
            cus.numOfItems = Random.Range(1, 5);
            lineOfCustomers.Add(cus);
        }
    }

    private void UpdateCustomerLine()
    {
        lineOfCustomers.Remove(curCustomer);
        SpeechBubbles[speechIndex].gameObject.SetActive(false);
        curCustomer.FinishedBeingServed();
        //Destroy(curCustomer.gameObject);
        if (lineOfCustomers.Count <= 0)
        {
            Completed();
            return;
        }
        
        curCustomer = lineOfCustomers[0];
        SpawnItems();
        for (int i = 0; i < lineOfCustomers.Count; i++)
        {
            lineOfCustomers[i].MoveTo(LinePositions[i].transform.position);
            //lineOfCustomers[i].transform.position = LinePositions[i].transform.position;
        }
        curCustomer.IsBeingServed(true);
        PickRandomSpeechBubble();

        GameManager.singleton.AddTime(7);
        GameManager.singleton.AddCash(100);
    }


    IEnumerator SetScanColor(float delay)
    {
        scanText.color = Color.green;
        recentScanCount++;
        yield return new WaitForSeconds(delay);
        recentScanCount--;
        if (recentScanCount == 0)
        {
            scanText.color = Color.white;
        }
    }

    public void ScanItem()
    {
        if (GameManager.singleton.gameFinished || GameManager.singleton.gameFailed) return;
        StartCoroutine(SetScanColor(1));
        beep.PlayOneShot(beep.clip);
        Debug.Log("scanned item");
        curCustomer.numOfItems--;
        if (curCustomer.numOfItems == 0)
        {
            UpdateCustomerLine();
        }
    }
    
    // do things with the grocery items now
    private void SpawnItems()
    {
        for (int i = 0; i < curCustomer.numOfItems; i++)
        {
            itemIndex = Random.Range(0, GroceryItems.Count);

            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                spawnBounds.max.y,   // Start from the top of the bounds
                Random.Range(spawnBounds.min.z, spawnBounds.max.z)
            );
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                // Place the item at the hit point on the ground
                GameObject obj = Instantiate(GroceryItems[itemIndex]);
                obj.GetComponent<Rigidbody>().MovePosition(hit.point + Vector3.up * (obj.GetComponent<Collider>().bounds.size.y * 0.5f));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.4f);
        Gizmos.DrawCube(spawnBounds.center, spawnBounds.size);
    }

    private void Completed()
    {
        GameManager.singleton.FinishMiniGame();
    }

    private void PickRandomSpeechBubble()
    {
        int randomNum = Random.Range(0, 3);
        if (randomNum == 0) willSpeak = true;
        else willSpeak = false;
        
        Debug.Log(randomNum);
        
        if(!willSpeak) return;

        for (int i = 0; i < SpeechBubbles.Count; i++)
        {
            speechIndex = Random.Range(0, SpeechBubbles.Count);
        }
        SpeechBubbles[speechIndex].gameObject.SetActive(true);
    }
    
    
}
