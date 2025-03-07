using UnityEngine.Pool;
using UnityEngine;

public class ObjectPooled : MonoBehaviour
{
    private IObjectPool<ObjectPooled> pool;

    // public property to give the projectile a reference to its ObjectPool
    public IObjectPool<ObjectPooled> Pool
    {
        get { return pool; }
        set => pool = value;
    }

    public void Release()
    {
        pool.Release(this);
    }
}
