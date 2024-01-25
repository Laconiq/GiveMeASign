using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public ProgressionManager progressionManager;
    [HideInInspector] public DialogueManager dialogueManager;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public UIManager uiManager;
    
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
        playerController = FindObjectOfType<PlayerController>();
        progressionManager = FindObjectOfType<ProgressionManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
        playerController.Initialize();
        progressionManager.Initialize();
        dialogueManager.Initialize();
        uiManager.Initialize();
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main menu is loading...");
    }
}
