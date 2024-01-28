using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [FormerlySerializedAs("progressionManager")] [HideInInspector] public EventManager eventManager;
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
        eventManager = FindObjectOfType<EventManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
        playerController.Initialize();
        eventManager.Initialize();
        dialogueManager.Initialize();
        uiManager.Initialize();
    }

    private void LoadMainMenu()
    {
        Debug.Log("Main menu is loading...");
    }
}
