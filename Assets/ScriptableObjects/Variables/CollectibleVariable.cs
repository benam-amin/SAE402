using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible Var", menuName = "ScriptableObjects/Variable/CollectibleVariable")]
public class CollectibleVariable : ScriptableObject
{
    public string title;
    public int value;
    public AudioClip audioClip;

    public Sprite sprite;

    [Multiline]
    public string DeveloperDescription = "";

    public PlaySoundAtEventChannel onPickUpAudio;
    public IntEventChannel onPickUpValue;

    public void PickItem(Vector3 position)
    {
        onPickUpAudio.Raise(audioClip, position);
        onPickUpValue.Raise(value);
    }
}