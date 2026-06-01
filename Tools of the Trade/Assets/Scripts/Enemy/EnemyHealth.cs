using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : HealthBase
{
    private void Awake()
    {
        healthZero.AddListener(AnnounceDeath);
        healthZero.AddListener(RemoveAllListeners);
        currentHP = maxHP;
    }

    protected void AnnounceDeath()
    {
        var enemySpawner = FindAnyObjectByType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.DecreaseEnemyCount();
        }
    }

    protected void RemoveAllListeners()
    {
        healthZero.RemoveAllListeners();
        var enemyHittableChild = GetComponentInChildren<EnemyHittable>();
        if (enemyHittableChild != null)
        {
            enemyHittableChild.onHit.RemoveAllListeners();
        }
        var enemyHittable = GetComponent<EnemyHittable>();
        if (enemyHittable != null)
        {
            enemyHittable.onHit.RemoveAllListeners();
        }
    }
}
