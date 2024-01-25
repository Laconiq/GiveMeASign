using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ProgressionManager progressionManager;
    
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
        if (sceneBuildIndex == 0)
            LoadMainMenu();
        else
            LoadGame();
    }
    
    private void LoadGame()
    {
        Debug.Log("Game is loading...");
        progressionManager.Initialize();
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main menu is loading...");
    }
}
