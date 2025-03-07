using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [Header("Broadcast event channels")]
    public BoolEventChannel onTogglePauseEvent;

    [Header("Listen to event channels")]
    public BoolEventChannel onDebugConsoleOpenEvent;

    bool isGamePaused = false;
    bool isDebugConsoleEnabled = false;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
    }

    private void OnEnable()
    {
        onDebugConsoleOpenEvent.OnEventRaised += TogglePauseDebug;
    }

    void Update()
    {
        if (!isDebugConsoleEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            TogglePause(isGamePaused);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        onTogglePauseEvent.Raise(false);
        pauseMenuUI.SetActive(false);
    }

    void Pause(bool displayPauseMenuUI = true)
    {
        Time.timeScale = 0f;
        onTogglePauseEvent.Raise(true);
        if (displayPauseMenuUI)
        {
            pauseMenuUI.SetActive(true);
        }
        Application.targetFrameRate = 30;
    }

    void TogglePause(bool pauseGame)
    {
        if (!pauseGame)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void TogglePauseDebug(bool pauseGame)
    {
        isDebugConsoleEnabled = !isDebugConsoleEnabled;
        if (!pauseGame)
        {
            Resume();
        }
        else
        {
            Pause(false);
        }
    }

    private void OnDisable()
    {
        onDebugConsoleOpenEvent.OnEventRaised -= TogglePauseDebug;
    }
}
