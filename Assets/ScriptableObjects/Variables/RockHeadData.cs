using UnityEngine;

[CreateAssetMenu(fileName = "New RockHeadData", menuName = "ScriptableObjects/Variable/RockHeadData")]
public class RockHeadData : ScriptableObject
{
    public float speed = 1200;
    [ReadOnlyInspector]
    public float mass = 1000;
    public float delayBetweenMoves = 2.25f;
    public LayerMask listContactsLayers;
    public float crushDistance = 0.65f;
    [Tooltip("Animation to play before moving")]
    public AnimationClip animationClip;
}