using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public FloatVariable maxHealth;

    [ReadOnlyInspector]
    public float currentHealth = 0f;

    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    public Animator animator;
    [ReadOnlyInspector]
    public Color originalColor;

    public Color hitColor = new Color(0.8207547f, 0.8207547f, 0.8207547f);

    // List of contact points when something collides with that GameObject
    private ContactPoint2D[] listContacts = new ContactPoint2D[1];

    [Header("Components to disable after specific event. E.g. : death")]
    public Behaviour[] listComponents;

    private float bounceFactorOnDeath = 5.25f;

    private void Start()
    {
        // If no max health is defined then the enemy heath is 1
        currentHealth = maxHealth != null ? maxHealth.CurrentValue : 1f;
        originalColor = spriteRenderer.color;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentHealth <= 0) return;

        other.GetContacts(listContacts);

        if (
            other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth) &&
            other.gameObject.CompareTag("Player") &&
            listContacts[0].normal.y > -0.5f
            )
        {
            playerHealth.TakeDamage(1f);
        }
    }

    public void Hurt()
    {
        StartCoroutine(TakeDamage(1f));

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (animator)
        {
            animator.SetTrigger("IsHit");
        }
        else
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator Die()
    {
        if (rb.bodyType != RigidbodyType2D.Dynamic)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        foreach (Behaviour component in listComponents)
        {
            component.enabled = false;
        }

        foreach (Collider2D collider in gameObject.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        rb.linearVelocity = Vector2.zero;
        rb.mass = 1;
        rb.gravityScale = 3;
        yield return null;
        Vector2 bounceForce = Vector2.up * bounceFactorOnDeath;
        rb.AddForce(bounceForce, ForceMode2D.Impulse);

        float startZAngle = transform.rotation.z;
        float current = 0;
        float duration = 0.85f;

        yield return null;
        while(current <= 1) {
            // https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
            float angle = Mathf.LerpAngle(startZAngle, 180, Mathf.Sin(current * Mathf.PI * 0.5f));
    
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);

            current += Time.fixedDeltaTime / duration;

            yield return null;
        }
    }

    void OnBecameInvisible()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 0.15f);
        }
    }
}
