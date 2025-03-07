using System.Collections;
using UnityEngine;

// https://www.youtube.com/watch?v=efYt5h2i_1A
public class LoopingBackground : MonoBehaviour
{
    public Renderer render;

    private Color startColor;
    public Color deathColor;

    [Header("Listen to event channels")]
    public VoidEventChannel onPlayerDeath;

    private void OnEnable()
    {
        onPlayerDeath.OnEventRaised += DieProxy;
    }

    private void Start() {
        startColor = render.material.GetColor("_Color");
    }

    private void DieProxy()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        render.material.SetColor("_Color", deathColor);
        yield return new WaitForSeconds(0.1f);
        render.material.SetColor("_Color", startColor);
    }

    private void OnDisable()
    {
        onPlayerDeath.OnEventRaised -= DieProxy;
    }
}
