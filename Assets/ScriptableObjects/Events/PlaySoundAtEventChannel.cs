using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Play Sound At Event", menuName = "ScriptableObjects/Events/PlaySoundAtEventChannel")]
public class PlaySoundAtEventChannel : ScriptableObject
{
    public UnityAction<AudioClip, Vector3> OnEventRaised;

	public void Raise(AudioClip sound, Vector3 position)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(sound, position);
	}
}
