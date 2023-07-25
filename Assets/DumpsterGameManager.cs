using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DumpsterGameManager : MonoBehaviour
{
    public List<GameObject> trashBags = new List<GameObject>();
    public List<Transform> spawnLocations = new List<Transform>();

    private int numOfObjects;

    private int trashIndex;
    private int spawnIndex;
    
    private void Awake()
    {
        numOfObjects = Random.Range(1, 4);
    }

    private void Start()
    {
        for (int i = 0; i < numOfObjects; i++)
        {
            trashIndex = i;
        }

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            Instantiate(trashBags[trashIndex], spawnLocations[i].transform.position,
                spawnLocations[i].transform.rotation);
        }
    }
}
