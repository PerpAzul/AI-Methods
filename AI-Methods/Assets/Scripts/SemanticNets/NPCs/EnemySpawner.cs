using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 4f;

    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        enemyPrefab.SetActive(true);
        Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemyPrefab.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
