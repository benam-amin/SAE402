using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    public CollectibleVariable data;
    public GameObject collectedEffect;
    public SpriteRenderer spriteRenderer;

    public UnityEvent onPickUp;

    private Vector3 startAngle;
    private float finalAngle;
    private float rotationOffset = 15f;
    private float oscillationSpeed = 1.5f;

    private void Awake()
    {
        spriteRenderer.sprite = data.sprite;

        startAngle = transform.eulerAngles;
    }

    private void Update()
    {
        finalAngle = startAngle.z + Mathf.Sin(Time.time * oscillationSpeed) * rotationOffset;
        transform.eulerAngles = new Vector3(startAngle.x, startAngle.y, finalAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Picked();
        }
    }

    private void Picked()
    {
        GameObject effect = Instantiate(collectedEffect, transform.position, transform.rotation);
        // Destroy effect after its animation ends playing
        Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        data.PickItem(transform.position);
        onPickUp?.Invoke();

        Destroy(gameObject);
    }
}
