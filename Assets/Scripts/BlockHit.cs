using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public Animator animator;
    public int numberHit = -1;
    private ContactPoint2D[] listContacts = new ContactPoint2D[];
    private void OnCollisionEnter2D (OnCollision2D collision) {
        collision.GetContacts(listContacts);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
