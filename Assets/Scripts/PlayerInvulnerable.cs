using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvulnerable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public bool isInvulnerable = false;

    public LayerMask layersToIgnoreAfterHit;

    public Animator animator;

    [SerializeField]
    private float invulnerableDuration = 2.5f;

    private List<int> listLayersIndexes = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        CreateListLayers();
        ToggleCollisions(gameObject.layer, false);
    }

    private void CreateListLayers()
    {
        for (int i = 0; i < 32; i++)
        {
            if (layersToIgnoreAfterHit == (layersToIgnoreAfterHit | (1 << i)))
            {
                listLayersIndexes.Add(i);
            }
        }
    }

    public IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        animator.SetTrigger("IsHit");
        ToggleCollisions(gameObject.layer, isInvulnerable);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        float timeElapsed = 0;
        while (timeElapsed < invulnerableDuration)
        {
            timeElapsed += Time.deltaTime;

            if (Time.frameCount % 8 == 0)
            {
                if (spriteRenderer.color.a == 1)
                {
                    spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                }
                else
                {
                    spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                }
            }

            yield return null;
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        isInvulnerable = false;
        ToggleCollisions(gameObject.layer, isInvulnerable);
    }

    public void ToggleCollisions(int gameObjectLayer, bool enabled)
    {
        foreach (var layerIndex in listLayersIndexes)
        {
            Physics2D.IgnoreLayerCollision(
                gameObjectLayer,
                layerIndex,
                enabled
            );
        }
    }
}
