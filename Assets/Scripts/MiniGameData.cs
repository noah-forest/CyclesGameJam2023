using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "MiniGame")]
public class MiniGameData : ScriptableObject
{
    public string gameName;
    public string description;
    public Scene miniGameScene;
    
    // bring data back
}
