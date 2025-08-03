using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject parentEnemies;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnRadius = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
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