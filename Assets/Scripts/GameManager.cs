using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public SceneAsset originalScene;

    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager)
        {
            Destroy(this.gameObject);
        }
        gameManager = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadMiniGame(MiniGameData data)
    {
        SceneManager.LoadScene(data.miniGameScene.name);
    }

    public void FinishMiniGame()
    {
        SceneManager.LoadScene(originalScene.name);
        
    }
}
