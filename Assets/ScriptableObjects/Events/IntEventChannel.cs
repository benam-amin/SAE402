using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Int Event", menuName = "ScriptableObjects/Events/IntEventChannel")]
public class IntEventChannel : ScriptableObject
{
    public UnityAction<int> OnEventRaised;

	public void Raise(int value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
