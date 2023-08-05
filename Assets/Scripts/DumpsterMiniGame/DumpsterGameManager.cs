using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DumpsterGameManager : MonoBehaviour
{
    private bool completed = false;
    public static DumpsterGameManager singleton;
    
    public TextMeshPro objText;

    public List<GameObject> trashBags = new List<GameObject>();
    public List<Transform> spawnLocations = new List<Transform>();

    public int numOfObjects;
    public int maxBags = 3;
    private int trashIndex;
    private int spawnIndex;

    private int count;
    
    private void Awake()
    {
        singleton = this;
        numOfObjects = Mathf.Clamp(Random.Range(1, GameManager.singleton.difficulty), 1, maxBags);
    }

    private void Start()
    {
        for (int i = 0; i < numOfObjects; i++)
        {
            trashIndex = Random.Range(0, trashBags.Count);
            Instantiate(trashBags[trashIndex], spawnLocations[i].transform.position,
                spawnLocations[i].transform.rotation);
        }
    }

    public void Completed()
    {
        if (!completed)
        {
            completed = true;
            GameManager.singleton.FinishMiniGame();
        }
    }
}
