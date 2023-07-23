using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;

    private void Start()
    {
        Customers customersInJson = JsonUtility.FromJson<Customers>(jsonFile.text);

        foreach (Customer customer in customersInJson.customers)
        {
            Debug.Log("Found Customer: " + customer.name);
        }
    }
}
