using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public int NumberOfEnemiesToSpawn = 5;
    public float SpawnDelay = 1f;


    // Start is called before the first frame update
    void Start()
    {
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
        // Spawn enemy
    }
}
