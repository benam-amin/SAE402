using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EndLevel : MonoBehaviour
{
    public ParticleSystem particles;
    public AudioClip audioClip;

    [Space (10)]
    [Header("Scene's name to load after the collider is triggered")]
    public string nextLevelName;
    [Space (10)]

    [Header("Broadcast event channels")]
    public StringEventChannel onLevelEnded;
    public PlaySoundAtEventChannel sfxAudioChannel;
    public CurrentSceneManager csm;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            if (nextLevelName != null)
            {
                particles.Play();
                sfxAudioChannel.Raise(audioClip, transform.position);
                SceneManager.LoadScene(nextLevelName);
            } else {
                Debug.LogError("Level missing");
            }
        }
    }
}
