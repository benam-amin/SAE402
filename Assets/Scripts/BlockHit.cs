using UnityEngine;
using System.Collections;

public class BlockHit : MonoBehaviour
{
    public Animator animator;
    public int numberHit = 1;
    public GameObject itemPrefab; // Declare itemPrefab
    private ContactPoint2D[] listContacts = new ContactPoint2D[1];
    public SpriteRenderer sr;
    public PlatformEffector2D platformEffector2D;
    public bool isHidden = false;

    // private void Start() {
    // Debug.Log("Start called");
    // }

    private void Awake() {
        Debug.Log("isHidden: " + isHidden);
        numberHit = 2;
        sr.enabled = !isHidden;  // Désactiver le SpriteRenderer si isHidden est vrai
        platformEffector2D.enabled = isHidden; // Activer PlatformEffector2D si isHidden est vrai
        animator.ResetTrigger("IsHit"); // Réinitialiser le trigger
    }


    public void OnCollisionEnter2D (Collision2D collision) {
        collision.GetContacts(listContacts);
        if (listContacts[0].normal.y > 0.5f && collision.gameObject.CompareTag("Player") && numberHit > 0) {
            StartCoroutine(Hit());
            if (numberHit <= 0) {
        }
        }

    }

    private IEnumerator Hit() {
        yield return null;
        sr.enabled = true;
        Debug.Log("Hit triggered");
        animator.SetTrigger("IsHit");
        numberHit--;
        animator.SetInteger("numberHit", numberHit);
        if (numberHit <= 0) {
            animator.SetTrigger ("IsInactive");
        }
         if (itemPrefab != null) {
            GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.GetComponent<Collectible>().canBeDestroyedOnContact = false;
            Vector3 endPosition = item.transform.localPosition + Vector3.up * 1.5f;
            yield return item.transform.MoveBackAndForth(endPosition);
            item.GetComponent<Collectible>().Picked();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
