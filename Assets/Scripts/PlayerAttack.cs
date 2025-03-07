using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public BoxCollider2D bc;
    public Rigidbody2D rb;
    public LayerMask listEnemiesLayers;
    public float checkDistance = 0.2f;
    public PlayerMovement playerMovement;
    RaycastHit2D hit;

    private void FixedUpdate()
    {
        hit = Physics2D.BoxCast(
            new Vector2(bc.bounds.center.x, bc.bounds.min.y - (checkDistance / 2)),
            new Vector2(bc.bounds.size.x * 0.8f, checkDistance),
            0,
            Vector2.down,
            0,
            listEnemiesLayers
        );

        if (hit.collider != null)
        {
            bool isAboveEnemy = hit.collider.transform.position.y < bc.bounds.min.y;

            EnemyDamageManager enemyDamageManager = hit.collider.GetComponent<EnemyDamageManager>();
            if (enemyDamageManager != null && isAboveEnemy && rb.linearVelocity.y <= 2.5f)
            {
                playerMovement.jumpCount = 0;
                enemyDamageManager.Hurt();
                playerMovement.Jump();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (bc != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x, bc.bounds.min.y - (checkDistance / 2)),
                new Vector2(bc.bounds.size.x * 0.8f, checkDistance)
            );
        }
    }
}
