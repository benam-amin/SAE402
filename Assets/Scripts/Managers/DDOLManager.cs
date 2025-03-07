using UnityEngine;
using UnityEngine.SceneManagement;
// https://stackoverflow.com/questions/35890932/unity-game-manager-script-works-only-one-time/35891919#35891919
// https://stackoverflow.com/questions/32306704/how-to-pass-data-and-references-between-scenes-in-unity
public class DDOLManager : MonoBehaviour
{
    public GameObject[] listObjects;
    public StringVariable currentSceneDevCompilation;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        foreach (var element in listObjects)
        {
            // Allow to keep gameobjects scene to scene with the same status
            DontDestroyOnLoad(element);
        }
    }

    private void Start()
    {
        #if UNITY_EDITOR
            SceneManager.LoadScene(currentSceneDevCompilation.CurrentValue);
        #else
            SceneManager.LoadScene(1);
        #endif
    }
}
