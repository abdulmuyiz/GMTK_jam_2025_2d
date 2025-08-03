using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject parentEnemies;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnRadius = 10f;
    public float minSpawnDelay = 0.5f;
    public float difficultyIncreaseRate = 0.1f; // how much faster per second

    private float currentSpawnDelay;
    private float spawnTimer;

    private void Start()
    {
        currentSpawnDelay = spawnInterval;
        spawnTimer = currentSpawnDelay;
    }

    void Update()
    {
        spawnTimer -= Time.fixedDeltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = currentSpawnDelay;
        }

        // Gradually decrease spawn delay over time
        currentSpawnDelay -= difficultyIncreaseRate * Time.deltaTime;
        currentSpawnDelay = Mathf.Clamp(currentSpawnDelay, minSpawnDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float spawnRadius = Random.Range(7f, 15f);
        Vector2 spawnPos = (Vector2)player.position + randomDir * spawnRadius;
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.SetParent(parentEnemies.transform);
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null) ai.player = player;
    }
}