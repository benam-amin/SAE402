using UnityEngine;

// Any GameObject with that component won't be in build version of the game
public class EditorModeOnly : MonoBehaviour
{
    [Tooltip("Any GameObject with that component won't be in build version of the game")]
    public bool isActive = false;
    private void Awake()
    {
#if UNITY_EDITOR
#else
    isActive = false;
#endif
        if(!isActive) {
            Destroy(gameObject);
        }
    }
}
