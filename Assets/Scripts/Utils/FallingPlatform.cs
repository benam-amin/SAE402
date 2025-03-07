using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Tooltip("Delay before the platform starts to fall")]
    public float fallDelay = 1.5f;
    private float destroyDelay = 2.25f;
    private Vector2 startPosition = Vector2.zero;

    public Rigidbody2D rb;

    public Animator animator;
    private bool isFalling = false;

    public ParticleSystem particleEmitter;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnBecameVisible()
    {
        particleEmitter.Play();
    }

    private void OnBecameInvisible()
    {
        particleEmitter.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);

        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y <= 0)
        {
            collision.gameObject.GetComponent<PlayerMovement>().isOnFallingPlatform = true;
            if (!isFalling)
            {
                StartCoroutine(Fall());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().isOnFallingPlatform = false;
        }
    }

    private IEnumerator Fall()
    {
        float current = 0;
        float duration = 0.15f;
        isFalling = true;

        while (current <= 1)
        {
            rb.MovePosition(
                Vector2.Lerp(
                    startPosition, 
                    startPosition + Vector2.down * 0.3f, 
                    Mathf.PingPong(current, 0.5f)
                )
            );
            current += Time.fixedDeltaTime / duration;

            yield return null;
        }

        animator.speed = 0.5f;
        yield return new WaitForSeconds(fallDelay);
        particleEmitter.Stop();
        animator.SetTrigger("Fall");
        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(destroyDelay);
        animator.speed = 1;
        isFalling = false;
        Invoke(nameof(Activate), destroyDelay);
        gameObject.SetActive(false);
        animator.ResetTrigger("Fall");
        transform.position = startPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Activate()
    {
        gameObject.SetActive(true);
    }
}
