using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New String Event", menuName = "ScriptableObjects/Events/StringEventChannel")]
public class StringEventChannel : ScriptableObject
{
    public UnityAction<string> OnEventRaised;

	public void Raise(string value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
