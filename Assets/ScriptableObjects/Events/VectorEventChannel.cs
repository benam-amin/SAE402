using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VectorEventChannel", menuName = "ScriptableObjects/Events/VectorEventChannel", order = 0)]
public class VectorEventChannel : ScriptableObject
{
    public UnityAction<Vector3> OnEventRaised;

	public void Raise(Vector3 value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
