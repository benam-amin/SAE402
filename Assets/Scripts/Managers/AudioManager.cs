using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public AudioMixerGroup soundEffectMixer;
    public AudioMixerGroup musicEffectMixer;
    public AudioClip[] playlist;
    private int musicIndex;
    private float volumeOnPaused = 0.35f;
    private float volumeOnPlay = 1f;
    private float volumeStep = 0.005f;

    [Header("Listen to event channels")]
    public PlaySoundAtEventChannel sfxAudioChannel;

    private void OnEnable()
    {
        sfxAudioChannel.OnEventRaised += PlayClipAt;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainAudioSource.clip = playlist[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainAudioSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    IEnumerator IncreaseVolume()
    {
        while (mainAudioSource.volume < volumeOnPlay)
        {
            mainAudioSource.volume += volumeStep;
            yield return null;
        }
    }

    IEnumerator DecreaseVolume()
    {
        while (mainAudioSource.volume > volumeOnPaused)
        {
            mainAudioSource.volume -= volumeStep;
            yield return null;
        }
    }

    void PlayNextMusic()
    {
        musicIndex = (musicIndex + 1) % playlist.Length;
        mainAudioSource.clip = playlist[musicIndex];
        mainAudioSource.outputAudioMixerGroup = musicEffectMixer;
        mainAudioSource.Play();
    }

    public void PlayClipAt(AudioClip clip, Vector3 position)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = soundEffectMixer;
        audioSource.Play();
        Destroy(tempGO, clip.length);
    }

    public void OnTogglePause(bool isGamePaused)
    {
        // AudioListener.pause = isGamePaused;
        if (isGamePaused)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseVolume());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseVolume());
        }
    }

    private void OnDisable()
    {
        sfxAudioChannel.OnEventRaised -= PlayClipAt;
    }
}
