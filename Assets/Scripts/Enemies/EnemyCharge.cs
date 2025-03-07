using System.Collections;
using UnityEngine;

public class EnemyCharge : MonoBehaviour
{
    [Header("Distance of sight")]
    public float sightLength = 4;

    public float knockbackStrength = 5.5f; // old value : 8.5

    private bool isCharging = false;
    private bool isOnScreen = false;
    private bool isMovingForward = false;
    private float obstacleDetectionLength = 0.2f;

    public SpriteRenderer spriteRenderer;

    private float checkTimer;
    public float delayBetweenCharges;
    public float speed;

    public Rigidbody2D rb;
    public Animator animator;

    public BoxCollider2D bc;

    private RaycastHit2D contact;

    public Knockback knockback;

    [Header("Layers")]
    public LayerMask obstacleLayers;
    public LayerMask targetLayers;

    [Header("Shake effect")]
    public CameraShakeEventChannel onCrushSO;
    public ShakeTypeVariable shakeInfo;


    private void Start()
    {
        checkTimer = delayBetweenCharges;
    }

    void Update()
    {
        if (!isCharging)
        {
            checkTimer += Time.deltaTime;
        }

        if (animator != null)
        {
            animator.SetFloat("VelocityX", rb.linearVelocity.x * transform.right.normalized.x);
        }
    }

    private void FixedUpdate()
    {
        CheckForTarget();
        CheckForObstacle();
        FlipCheck();
        CheckForBottomTarget();

        if (contact.collider != null && Mathf.Abs(rb.linearVelocity.x) > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        }
    }

    private void FlipCheck()
    {
        if (isCharging || rb.linearVelocity.y != 0)
        {
            return;
        }

        Vector3 startCast = new Vector2(bc.bounds.center.x, bc.bounds.center.y);
        float offset = (sightLength / 4) + (bc.bounds.size.x / 2);
        Vector3 endCast = new Vector2(bc.bounds.center.x - (transform.right.normalized.x * offset), bc.bounds.center.y);

        RaycastHit2D hit = Physics2D.Linecast(startCast, endCast, targetLayers);

        if (hit.collider != null)
        {
            Flip();
        }
    }

    private void CheckForObstacle()
    {
        contact = GetContact();
        if (!isCharging || !isMovingForward)
        {
            return;
        }


        if (contact.collider != null && isMovingForward)
        {
            StopAllCoroutines();
            Stop(contact.collider);
        }
    }

    private RaycastHit2D GetContact()
    {
        Vector2 originCast = new Vector2(
                    bc.bounds.center.x + (transform.right.normalized.x * (bc.bounds.size.x / 2)) + (transform.right.normalized.x * obstacleDetectionLength / 2),
                    bc.bounds.min.y + (bc.bounds.size.y / 4)
                );
        Vector3 sizeCast = new Vector2(obstacleDetectionLength, bc.bounds.size.y * 1 / 2);

        return Physics2D.BoxCast(
            originCast,
            sizeCast,
            0,
            transform.right.normalized,
            0,
            obstacleLayers
        );
    }

    private void CheckForTarget()
    {
        if (isCharging || checkTimer < delayBetweenCharges)
        {
            return;
        }

        float offset = sightLength + (bc.bounds.size.x / 2);

        Vector3 startCast = new Vector2(bc.bounds.center.x + (1.15f * transform.right.normalized.x), bc.bounds.center.y);
        Vector3 endCast = new Vector2(bc.bounds.center.x + (transform.right.normalized.x * offset), bc.bounds.center.y);

        RaycastHit2D hit = Physics2D.Linecast(startCast, endCast, targetLayers);

        if (hit.collider != null)
        {
            StartCoroutine(Charge(hit.collider.transform.position));
        }
    }

    private void CheckForBottomTarget()
    {
        Collider2D hit = Physics2D.OverlapBox(
            new Vector2(bc.bounds.center.x, bc.bounds.min.y - (0.15f / 2)),
            new Vector2(bc.size.x * 0.9f, 0.15f),
            0,
            targetLayers
        );

        if (hit != null)
        {
            PlayerContacts playerContacts = hit.transform.GetComponent<PlayerContacts>();
            if (playerContacts.hasBottomContact && rb.linearVelocity.y < 0)
            {
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(float.MaxValue);
            }
        }
    }

    private IEnumerator Charge(Vector3 target)
    {
        isCharging = true;

        spriteRenderer.color = Color.red;

        float dirX = (target - transform.position).normalized.x;
        float current = 0;
        float moveBackDuration = 0.85f;

        WaitForFixedUpdate waitInterval = new WaitForFixedUpdate();

        while (current <= 1)
        {
            current += Time.deltaTime / moveBackDuration;
            // Minus 1 because the enemy moveback firstAssets/Scripts/Enemies/EnemyCharge.cs
            var dir = (transform.position * (dirX * -1)).normalized;
            rb.linearVelocity = new Vector2(dir.x, rb.linearVelocity.y);

            yield return waitInterval;
        }

        rb.linearVelocity = Vector2.zero;
        isMovingForward = true;

        yield return new WaitForSeconds(0.15f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
        rb.AddForce(new Vector2(speed * dirX, rb.linearVelocity.y) * rb.mass, ForceMode2D.Impulse);
    }

    void Stop(Collider2D collider)
    {
        animator.SetTrigger("IsHit");
        // Fallback if the charge is interrupted
        spriteRenderer.color = new Color(1, 1, 1, 1);
        isMovingForward = false;

        if (collider.TryGetComponent<Knockback>(out Knockback knockbackContact))
        {
            Vector2 direction = (transform.position - collider.transform.position).normalized * -1f;
            direction.y = 0;
            knockbackContact.Apply(direction, knockbackStrength);
        }

        if (knockback != null)
        {
            knockback.Apply(
                new Vector2(Mathf.Sign(-transform.right.normalized.x) * 0.15f, 0.35f),
                knockbackStrength * 1.5f
            );
        }

        if (collider.CompareTag("Player"))
        {
            PlayerContacts playerContacts = collider.transform.GetComponent<PlayerContacts>();
            if (playerContacts.hasLeftContact || playerContacts.hasRightContact)
            {
                PlayerHealth playerHealth = collider.transform.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(float.MaxValue);
            }
        }
        if (isOnScreen)
        {
            onCrushSO.Raise(shakeInfo);
        }

        isCharging = false;
        checkTimer = 0;
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    void OnDrawGizmos()
    {
        if (bc != null)
        {
            if (isCharging)
            {
                Gizmos.color = Color.magenta;
                Vector2 startCast = new Vector2(
                    bc.bounds.center.x + (transform.right.normalized.x * (bc.bounds.size.x / 2)) + (transform.right.normalized.x * obstacleDetectionLength / 2),
                    bc.bounds.min.y + (bc.bounds.size.y / 4)
                );
                Gizmos.DrawWireCube(
                    startCast,
                    new Vector2(obstacleDetectionLength, bc.bounds.size.y * 1 / 2)
                );
            }
            else
            {
                Gizmos.color = Color.blue;
                float targetSightOffset = sightLength + (bc.bounds.size.x / 2);
                Gizmos.DrawLine(
                    new Vector2(bc.bounds.center.x + (1.15f * transform.right.normalized.x), bc.bounds.center.y),
                    new Vector2(bc.bounds.center.x + (transform.right.normalized.x * targetSightOffset), bc.bounds.center.y)
                );

                Gizmos.color = Color.magenta;
                float offset = (sightLength / 4) + (bc.bounds.size.x / 2);
                Gizmos.DrawLine(
                    new Vector2(bc.bounds.center.x, bc.bounds.center.y),
                    new Vector2(bc.bounds.center.x - (transform.right.normalized.x * offset), bc.bounds.center.y)
                );
            }

            Gizmos.color = Color.black;
            float crushOffset = 0.15f / 2;
            Gizmos.DrawWireCube(
                new Vector2(bc.bounds.center.x, bc.bounds.min.y - crushOffset),
                new Vector2(bc.size.x * 0.9f, 0.15f)
            );
        }
    }

    void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    void OnBecameVisible()
    {
        isOnScreen = true;
    }
}
