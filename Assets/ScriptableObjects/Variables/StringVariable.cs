using UnityEngine;

[CreateAssetMenu(fileName = "New String Var", menuName = "ScriptableObjects/Variable/StringVariable")]
public class StringVariable : ScriptableObject
{
    public string CurrentValue;

    [Multiline]
    public string DeveloperDescription = "";
}