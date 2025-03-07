using UnityEngine;

public class DevScenePreloadManager : MonoBehaviour
{
    #if UNITY_EDITOR
    public StringVariable currentSceneDevCompilation;
    void Awake()
    {
        GameObject check = GameObject.Find("Bootstrap");
        if (check == null)
        { 
            currentSceneDevCompilation.CurrentValue = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            UnityEngine.SceneManagement.SceneManager.LoadScene("_Preload"); 
        }
    }
    #endif
}
