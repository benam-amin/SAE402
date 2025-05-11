using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [Tooltip("Define where the player will spawn if there is an issue"), ReadOnlyInspector]
    public Vector3 currentSpawnPosition;

    [Tooltip("Define where the player started the level"), ReadOnlyInspector]
    public Vector3 initialSpawnPosition;
    public Vector3Variable lastCheckpointPosition;
    public LayerMask groundLayer;
    public float stayTime = 0.5f;

    private float groundTimer = 0f;
    private bool isOnGround = false;
    private bool wasOnGround = false;
    private Vector3 lastSafePosition;

    private void Awake()
    {
        if (lastCheckpointPosition.CurrentValue != null) {
            transform.position = (Vector3) lastCheckpointPosition.CurrentValue;
        } else {
            lastCheckpointPosition.CurrentValue = transform.position;
        }
        currentSpawnPosition = gameObject.transform.position;
        initialSpawnPosition = gameObject.transform.position;
        lastSafePosition = transform.position;
    }

    private void Update()
    {
        if (isOnGround)
        {
            groundTimer += Time.deltaTime;
            if (groundTimer >= stayTime)
            {
                lastSafePosition = transform.position;
                wasOnGround = true;
            }
        }
        else
        {
            groundTimer = 0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isOnGround = false;

            if (wasOnGround)
            {
                lastCheckpointPosition.CurrentValue = lastSafePosition;
                currentSpawnPosition = lastSafePosition;
                wasOnGround = false;
            }
        }
    }
}
