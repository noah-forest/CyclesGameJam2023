using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PunchOutGameManager : MonoBehaviour
{
    public TextMeshPro screenText;

    public GameObject card;
    public Transform startingPos;

    [SerializeField] private resetCardPosition exitBounds1;
    [SerializeField] private resetCardPosition exitBounds2;
    [SerializeField] private CanScan resetTrigger;
    [SerializeField] private ScannedCard scanned;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip beep;
    [SerializeField] private AudioClip fail;
    private Rigidbody rb;

    private int scanTarget;
    private int scanCount;
    private int tempScan;
    private bool finished = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        scanTarget = Random.Range(1, 5);
        rb = card.GetComponent<Rigidbody>();
        //Debug.Log("scans needed: " + scanTarget);
    }

    private void Start()
    {
        if (GameMusicPlayer.Instance)
        {
            GameMusicPlayer.Instance.PickRandomSong();
        }
        scanned.OnScan.AddListener(OnScan);
        exitBounds1.OutOfBounds.AddListener(OnExitBounds);
        exitBounds2.OutOfBounds.AddListener(OnExitBounds);
    }

    private void OnScan()
    {
        if (!resetTrigger.canBeScanned) return;
        if(finished) return;
        scanCount++;
        if (scanCount >= scanTarget)
        {
            finished = true;
            _audioSource.PlayOneShot(beep);
            screenText.color = Color.green;
            GameManager.singleton.FinishMiniGame();
        }
        else
        {
            StartCoroutine(FailedScan(0.5f));
        }

        if (resetTrigger.canBeScanned && scanned.cardScanned)
        {
            resetTrigger.canBeScanned = false;
        }
    }

    private void OnExitBounds()
    {
        card.transform.position = startingPos.transform.position;
        rb.velocity = Vector3.zero;
    }
    
    IEnumerator FailedScan(float delay)
    {
        screenText.color = Color.red;
        screenText.SetText("Failed!");
        _audioSource.PlayOneShot(fail);
        tempScan++;
        yield return new WaitForSeconds(delay);
        tempScan--;
        if (tempScan == 0)
        {
            screenText.color = Color.white;
            screenText.SetText("Swipe!");
        }

        if (!finished) yield break;
        screenText.SetText("Success!");
        screenText.color = Color.green;
    }
}
