using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public SpriteRenderer sr;

    public IEnumerator Show(float duration = 1)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / duration)
        {
            sr.color = new Color(1, 1, 1, i);
            yield return null;
        }
        yield return null;
    }

    public IEnumerator Hide(float duration = 1)
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime / duration)
        {
            sr.color = new Color(1, 1, 1, i);
            yield return null;
        }
        yield return null;
    }
}
