using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameMusicPlayer : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioSource _audioSource;
    [SerializeField] private float _songsPlayed;
    [SerializeField] private bool[] _beenPlayed;
    
    #region singleton

    public static GameMusicPlayer Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _beenPlayed = new bool[songs.Length];
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        
        if (!_audioSource.isPlaying)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }

        ResetShuffle();
    }

    public void PickRandomSong()
    {
        _audioSource.Stop();
        ChangeSong(Random.Range(0, songs.Length));
    }
    private void ChangeSong(int songPicked)
    {
        if (!_beenPlayed[songPicked])
        {
            _songsPlayed++;
            _beenPlayed[songPicked] = true;
            _audioSource.clip = songs[songPicked];
            _audioSource.Play();
        } else
            _audioSource.Stop();
    }

    private void ResetShuffle()
    {
        if (_songsPlayed == songs.Length)
        {
            _songsPlayed = 0;
            for (var i = 0; i < songs.Length; i++)
            {
                if (i == songs.Length)
                {
                    break;
                }
                else
                {
                    _beenPlayed[i] = false;
                }
            }
        }
    }
}
