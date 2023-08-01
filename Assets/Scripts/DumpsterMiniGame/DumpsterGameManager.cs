using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DumpsterGameManager : MonoBehaviour
{
    private static DumpsterGameManager _instance;
    public static DumpsterGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DumpsterGameManager>();
            }
            return _instance;
        }
    }
    
    
    public TextMeshPro objText;

    public List<GameObject> trashBags = new List<GameObject>();
    public List<Transform> spawnLocations = new List<Transform>();

    public int numOfObjects;

    private int trashIndex;
    private int spawnIndex;

    private int count;
    
    private void Awake()
    {
        numOfObjects = Random.Range(1, 4);
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
        objText.SetText("Finished!");
        objText.color = Color.green;
        GameManager.singleton.FinishMiniGame();
    }
}
