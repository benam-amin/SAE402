using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    public bool isDebugConsoleOpened = false;

    [Header("Listen to events")]
    public StringEventChannel onLevelEnded;
    
    [Header("Scene's name to load after the start button is triggered")]
    public string startLevel;
    [Space (10)]
    
    public BoolEventChannel onDebugConsoleOpenEvent;
    public Vector3Variable playerPosition;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        onLevelEnded.OnEventRaised += LoadScene;
        onDebugConsoleOpenEvent.OnEventRaised += OnDebugConsoleOpen;
    }

    public void LoadScene(string sceneName)
    {
        if (UtilsScene.DoesSceneExist(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log($"Unknown scene named {sceneName}. Please add the scene to the build settings.");
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (UtilsScene.DoesSceneExist(sceneIndex))
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.Log($"Unknown scene with index {sceneIndex}. Please add the scene to the build settings.");
        }
    }

    public void RestartLevel()
    {
        playerPosition.CurrentValue = null;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public static void RestartLastCheckpoint()
    {
        Debug.Log("RestartLastCheckpoint");
        // Refill life to full
        // Position to last checkpoint
        // Remove menu
        // Reset Rigidbody
        // Reactivate Player movements
        // Reset Player's rotation
    }

    public static void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void MainMenu()
    {
        Debug.Log("Retour au menu demandé");
        SceneManager.LoadScene("MainMenu");
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDisable()
    {
        onLevelEnded.OnEventRaised -= LoadScene;
        onDebugConsoleOpenEvent.OnEventRaised -= OnDebugConsoleOpen;
    }

    private void OnDebugConsoleOpen(bool debugConsoleOpened)
    {
        isDebugConsoleOpened = debugConsoleOpened;
    }
}
