using UnityEngine;

public class EnemyUnshelled : MonoBehaviour
{
    public Animator animator;

    public GameObject unshelledPrefab;
    public GameObject shellPrefab;

    public void Hurt()
    {
        animator.SetTrigger("IsHit");
        Unshell();
    }

    private void Unshell()
    {
        Destroy(gameObject);

        if (unshelledPrefab != null)
        {
            GameObject unshelledEnemy = Instantiate(unshelledPrefab, transform.position, new Quaternion(0, transform.rotation.y, 0, 0));
            unshelledEnemy.GetComponent<Enemy>().Hurt();
        }

        Instantiate(
            shellPrefab,
            transform.position,
            new Quaternion(0, transform.rotation.y, 0, 0)
        );
    }
}
