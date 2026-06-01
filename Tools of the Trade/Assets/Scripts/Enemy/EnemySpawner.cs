using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int timmyKickCount = 1;
    [SerializeField] private int timmyPunchCount = 1;
    [SerializeField] private int timmyBothCount = 1;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject timmyKickPrefab;
    [SerializeField] private GameObject timmyPunchPrefab;
    [SerializeField] private GameObject timmyBothPrefab;

    public int TotalEnemies = 0;
    float elapsedTime = 0f;
    
    void Start()
    {
        BuildSpawnPool();
    }

    private List<GameObject> spawnPool = new List<GameObject>();

    // Call this once during setup (Start, or whenever counts are finalized)
    private void BuildSpawnPool()
    {
        spawnPool.Clear();

        for (int i = 0; i < timmyKickCount; i++)
            spawnPool.Add(timmyKickPrefab);

        for (int i = 0; i < timmyPunchCount; i++)
            spawnPool.Add(timmyPunchPrefab);

        for (int i = 0; i < timmyBothCount; i++)
            spawnPool.Add(timmyBothPrefab);

        // Fisher-Yates shuffle
        for (int i = spawnPool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spawnPool[i], spawnPool[j]) = (spawnPool[j], spawnPool[i]);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPool.Count == 0)
            return;

        SpawnAtRandomPoint(spawnPool[0]);
        spawnPool.RemoveAt(0);
        TotalEnemies++;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnInterval)
        {
            SpawnEnemy();
            elapsedTime = 0f;
        }
    }

    private void SpawnAtRandomPoint(GameObject enemyPrefab)
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<EnemyBehaviour>();
    }
}
