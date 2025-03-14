using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D bc2d;
    public Vector3Variable lastCheckpointPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerSpawn playerSpawn = collision.GetComponent<PlayerSpawn>();
            if (playerSpawn != null)
            {
                lastCheckpointPosition.CurrentValue = transform.position;//mettre valeur en nul quand niveau fini
                playerSpawn.currentSpawnPosition = transform.position;
                bc2d.enabled = false;
            }
        }
    }
}
