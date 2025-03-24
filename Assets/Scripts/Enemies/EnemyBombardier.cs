using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class FlyingEnemyDropping : MonoBehaviour
{
    public Transform dropPoint;
    public GameObject bulletGameObject;
    public float dropInterval = 1f;

    private void Start()
    {
        StartCoroutine(DropBullets());
    }

    private IEnumerator DropBullets()
    {
        while (true)
        {
            DropBullet();
            yield return new WaitForSeconds(dropInterval);
        }
    }

    private void DropBullet()
    {
        // ObjectPooled pooledBullet = bulletPooling.Get("bullet");
        // if (pooledBullet == null) return;

        // GameObject bullet = pooledBullet.gameObject;
        // bullet.transform.position = dropPoint.position;
        // bullet.SetActive(true);

        Instantiate(bulletGameObject, dropPoint.position, Quaternion.identity);
    }
}
