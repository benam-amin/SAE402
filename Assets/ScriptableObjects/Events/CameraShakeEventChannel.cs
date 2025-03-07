using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Camera Shake Event", menuName = "ScriptableObjects/Events/CameraShakeEventChannel")]
public class CameraShakeEventChannel : ScriptableObject
{
    public UnityAction<ShakeTypeVariable> OnEventRaised;

	public void Raise(ShakeTypeVariable value)
	{
		// We wheck if someone is really listening to our event
		if (OnEventRaised != null) {
			OnEventRaised.Invoke(value);
		} else {
			Debug.LogWarning("An event of type " + GetType().Name + " was raised without any listener");
		}
	}
}
