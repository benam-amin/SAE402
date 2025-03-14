
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Variable", menuName = "Scriptable Objects/Variable/Vector3Variable")]
public class Vector3Variable : ScriptableObject
{
    public Vector3? CurrentValue;
    [Multiline]
    public string DeveloperDescription = "";
}
