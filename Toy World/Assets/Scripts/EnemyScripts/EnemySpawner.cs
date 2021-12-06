using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int NumberOfEnemiesToSpawn = 5;
    [SerializeField] private float SpawnDelay = 1f;
    [SerializeField] private float spawnDistance = 5f;

    private Transform player;
    private GameObject enemyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("CoreBlock").transform;
        enemyPrefab = Resources.Load("Enemy") as GameObject;

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        int spawnedEnemies = 0;

        while (spawnedEnemies < NumberOfEnemiesToSpawn)
        {
            SpawnEnemy();

            spawnedEnemies++;

            yield return Wait;
        }
    }

    private void SpawnEnemy()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        int vertIndex = Random.Range(0, triangulation.vertices.Length);

        while (Vector3.Distance(triangulation.vertices[vertIndex], player.transform.position) < spawnDistance)
        {
            vertIndex = Random.Range(0, triangulation.vertices.Length);
        }

        NavMeshHit hit;

        if (NavMesh.SamplePosition(triangulation.vertices[vertIndex], out hit, 2f, NavMesh.AllAreas))
        {
            Instantiate(enemyPrefab, hit.position, Quaternion.identity);
        }
    }
}
