using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.gameManager.FinishMiniGame();
        }
    }
}
