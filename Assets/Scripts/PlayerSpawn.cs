using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Tooltip("Define where the player will spawn if there is an issue"), ReadOnlyInspector]
    public Vector3 currentSpawnPosition;

    [Tooltip("Define where the player started the level"), ReadOnlyInspector]
    public Vector3 initialSpawnPosition;
    public Vector3Variable lastCheckpointPosition;
    private void Awake()
    {     
        if (lastCheckpointPosition.CurrentValue != null) {
            transform.position = (Vector3) lastCheckpointPosition.CurrentValue;
        } else {
            lastCheckpointPosition.CurrentValue = transform.position;
        }
        currentSpawnPosition = gameObject.transform.position;
        initialSpawnPosition = gameObject.transform.position;
    }
}
