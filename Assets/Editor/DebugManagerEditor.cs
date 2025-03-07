using UnityEditor;
using UnityEngine.SceneManagement;

public class DebugManagerEditor
{
    [MenuItem("**Debug**/Restart Scene _F5")]
    private static void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Shift + M
    [MenuItem("**Debug**/Restart Last Checkpoint #M")]
    private static void RestartLastCheckpoint()
    {
        CurrentSceneManager.RestartLastCheckpoint();
    }

    // Shift + L
    [MenuItem("**Debug**/Quit game #L")]
    private static void QuitGame()
    {
        CurrentSceneManager.QuitGame();
    }

    // Shift + O
    [MenuItem("**Debug**/Load Debug scene #O")]
    private static void LoadDebugScene()
    {
        SceneManager.LoadScene("Debug");
    }
}
