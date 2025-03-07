using UnityEngine;

[CreateAssetMenu(fileName = "New Float Var", menuName = "ScriptableObjects/Variable/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    public float CurrentValue;

    [Multiline]
    public string DeveloperDescription = "";
}