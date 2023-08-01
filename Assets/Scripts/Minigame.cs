using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "MiniGame")]
public class Minigame : ScriptableObject
{
    public Object scene;
    public string name;
    public string description;
    public float timerLength = 2000;
    public float cashReward;
    public float cashPenalty;
    // bring data back
}
