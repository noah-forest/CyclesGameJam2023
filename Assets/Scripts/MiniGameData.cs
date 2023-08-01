using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "MiniGame")]
public class MiniGameData : ScriptableObject
{
    public Object scene;
    public string name;
    public string description;
    
    // bring data back
}
