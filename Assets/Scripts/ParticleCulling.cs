using UnityEngine;

public class ParticleCulling : MonoBehaviour
{
    public ParticleSystem ps;
    private void Awake() {
        ps.Stop();
    }
    private void OnBecameVisible()
    {
        ps.Play();
    }
    private void OnBecameInvisible()
    {
        ps.Stop();
    }
}
