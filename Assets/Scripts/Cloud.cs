using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;
    [SerializeField]
    private SpriteRenderer sr;
    public Sprite[] listSprites;
    private float minSpeed = 0.05f;
    private float maxSpeed = 0.95f;

    [SerializeField]
    private Fade fade;

    [HideInInspector]
    public Vector2 endPos = Vector2.zero;

    private bool isHiding = false;

    void Update()
    {
        Vector2 direction = speed * Time.deltaTime * Vector2.right;
        transform.Translate(direction, Space.World);

        if (transform.position.x > endPos.x && !isHiding)
        {
            StartCoroutine(Hide());
        }
    }

    IEnumerator Hide()
    {
        isHiding = true;
        yield return StartCoroutine(fade.Hide());
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isHiding = false;
        speed = Random.Range(minSpeed, maxSpeed);
        sr.sprite = listSprites[Random.Range(0, listSprites.Length)];
        sr.color = Color.white;
    }
}
