using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    public Enemy enemy;

    [Header("Extra non mandatory components")]
    public EnemySplitting enemySplitting;
    public EnemyUnshelled enemyUnshelled;

    public void Hurt()
    {
        if (enemy != null)
        {
            enemy.Hurt();
        }

        if (enemySplitting != null)
        {
            enemySplitting.Hurt();
        }

        if (enemyUnshelled != null)
        {
            enemyUnshelled.Hurt();
        }
    }
}
