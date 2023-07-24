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
    public List<GameObject> BeltPositions = new List<GameObject>();
    //
    private List<Customer> lineOfCustomers = new List<Customer>();

    private int numOfCustomers;
    private int listIndex;
    private int itemIndex;
    private Customer curCustomer;

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
            Instantiate(GroceryItems[itemIndex], BeltPositions[i].transform.position,
                BeltPositions[i].transform.rotation);
        }
    }

    private void Completed()
    {
        Debug.Log("Finished checking out customers!");
    }
}
