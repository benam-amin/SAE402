using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement management")]
    public float speed = 0.5f;
    private bool isIdle = false;

    private float idleTime;

    [Tooltip("Define how long the enemy will move")]
    public float moveDuration = 5f;

    private WaitForSeconds waitMoveDuration;
    private WaitForSeconds waitIdleTime;
    [Tooltip("If false, the enemy will move endlessly back and forth")]
    public bool canWait = true;
    [Tooltip("If false, the enemy will flip when he doesn't touch the ground, if true, the enemy will be able to fly")]
    public bool canFly = false;

    [Header("Collision detection")]
    public LayerMask obstacleLayersMask;
    public float obstacleDetectionLength = 0.15f;
    public BoxCollider2D bc;
    [UnityEngine.Serialization.FormerlySerializedAs("groundCheckRadius")]
    public float obstacleCheckRadius = 1f;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        enabled = false;
    }

    private void Start()
    {
        if (canWait)
        {
            idleTime = Mathf.Round(moveDuration / 2.5f);

            waitMoveDuration = new WaitForSeconds(moveDuration);
            waitIdleTime = new WaitForSeconds(idleTime);

            StartCoroutine(ChangeState());
        }
    }

    private void Update()
    {
        animator.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocity.x));
    }

    private void FixedUpdate()
    {
        if(canFly){
            if (HasCollisionWithObstacle())
            {
                Flip();
            }
        } else {
            if (HasCollisionWithObstacle() || HasNotTouchedGround())
            {
                Flip();
            }
        }

        if (isIdle)
        {
            Idle();
        }
        else
        {
            Move();
        }
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            // Enemy will walk during X seconds...
            isIdle = false;
            yield return waitMoveDuration;

            // ...then wait during X seconds...
            isIdle = true;
            yield return waitIdleTime;
        }
    }

    private void Idle()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(speed * Mathf.Sign(transform.right.normalized.x), rb.linearVelocity.y);
    }

    public bool HasCollisionWithObstacle()
    {
        Vector2 startCast = new Vector2(
            bc.bounds.center.x + (transform.right.normalized.x * (bc.bounds.size.x / 2)),
            bc.bounds.center.y
        );
        Vector2 endCast = new Vector2(startCast.x + (transform.right.normalized.x * obstacleDetectionLength), startCast.y);

        RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);

        return hitObstacle.collider != null;
    }

    public bool HasNotTouchedGround()
    {
        Vector2 center = new Vector2(
            bc.bounds.center.x + (transform.right.normalized.x * (bc.bounds.size.x / 2)),
            bc.bounds.min.y
        );

        return !Physics2D.OverlapCircle(center, obstacleCheckRadius, obstacleLayersMask);
    }

    void OnDrawGizmos()
    {
        if (bc != null)
        {
            Gizmos.DrawWireSphere(
                new Vector2(
                    bc.bounds.center.x + (transform.right.normalized.x * (bc.bounds.size.x / 2)),
                    bc.bounds.min.y
                ),
                obstacleCheckRadius
            );

            Gizmos.color = Color.green;
            Vector2 startCast = new Vector2(
                bc.bounds.center.x + (transform.right.normalized.x * (bc.bounds.size.x / 2)),
                bc.bounds.center.y
            );
            Gizmos.DrawLine(
                startCast,
                new Vector2(startCast.x + (transform.right.normalized.x * obstacleDetectionLength), startCast.y)
            );
        }
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (!canFly)
        {
            // On arrête l'ennemi uniquement s'il ne peut pas voler
            Idle();
            enabled = false;
        }
    }
}