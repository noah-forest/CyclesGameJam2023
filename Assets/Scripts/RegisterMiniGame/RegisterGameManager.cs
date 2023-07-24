using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<GameObject> GroceryItems = new List<GameObject>();
    public List<GameObject> LinePositions = new List<GameObject>();
    //
    private List<Customer> lineOfCustomers = new List<Customer>();

    private int numOfCustomers;
    private int listIndex;
    private int itemIndex;
    private Customer curCustomer;
    private AudioSource beep;
    public Bounds spawnBounds;

    private void Start()
    {
        beep = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        _instance = this;
        CreateCustomersInLine();
        curCustomer = lineOfCustomers[0];
        SpawnItems();
    }

    private void CreateCustomersInLine()
    {
        numOfCustomers = Random.Range(1, LinePositions.Count+1);
        for (int i = 0; i < numOfCustomers; i++)
        {
            listIndex = Random.Range(0, Customers.Count);
            Customer cus = Instantiate(Customers[listIndex], LinePositions[i].transform.position, LinePositions[i].transform.rotation);
            cus.numOfItems = Random.Range(1, 4);
            lineOfCustomers.Add(cus);
        }
    }

    private void UpdateCustomerLine()
    {
        lineOfCustomers.Remove(curCustomer);
        Destroy(curCustomer.gameObject);
        if (lineOfCustomers.Count <= 0)
        {
            Completed();
            return;
        }
        
        curCustomer = lineOfCustomers[0];
        SpawnItems();
        for (int i = 0; i < lineOfCustomers.Count; i++)
        {
            lineOfCustomers[i].transform.position = LinePositions[i].transform.position;
        }
    }

    public void ScanItem()
    {
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
                obj.GetComponent<Rigidbody>().MovePosition(hit.point + Vector3.up * obj.GetComponent<Collider>().bounds.size.y * 0.5f);
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
        Debug.Log("Finished checking out customers!");
    }
}
