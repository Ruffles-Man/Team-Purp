using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : HealthBase
{
    private void Awake()
    {
        healthZero.AddListener(AnnounceDeath);
    }

    protected void AnnounceDeath()
    {
        FindAnyObjectByType<EnemySpawner>().DecreaseEnemyCount();
    }
}
