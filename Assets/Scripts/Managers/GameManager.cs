using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
            Destroy(gameObject);
    }

    private void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneBuildIndex = scene.buildIndex;
        switch (sceneBuildIndex)
        {
            case 0:
                LoadMainMenu();
                break;
            case 1:
                LoadGame();
                break;
        }
    }
    
    private void LoadGame()
    {
        Debug.Log("Game is loading...");
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main menu is loading...");
    }
}
