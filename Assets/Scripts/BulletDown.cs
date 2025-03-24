using System.Collections;
using UnityEngine;

public class BulletDown : MonoBehaviour
{
    public Rigidbody2D rb;
    public float delayBeforeAutodestruction = 2.5f;
    public Animator animator;
    public float damage = 1f;
    private Coroutine autoDestroyCoroutine;

    IEnumerator AutoDestroy(float duration = 0)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision avec : " + other.gameObject.name);
        if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
        
        //Ne pas oublier de définir les plateformes avec le tag "Platforms".
        if (other.gameObject.CompareTag("Platforms"))
        {
            // animator.SetTrigger("IsCollided");
            Destroy(gameObject);
            Debug.Log("Destruction");
        }
    }

}
