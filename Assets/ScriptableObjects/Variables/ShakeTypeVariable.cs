using UnityEngine;

[CreateAssetMenu(fileName = "New Shake Type Var", menuName = "ScriptableObjects/Variable/ShakeTypeVariable")]
public class ShakeTypeVariable : ScriptableObject
{
    public float Duration;
    public float Magnitude;

    [Multiline]
    public string DeveloperDescription = "";
}