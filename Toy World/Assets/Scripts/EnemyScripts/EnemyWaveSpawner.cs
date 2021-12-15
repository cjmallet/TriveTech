using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public int waveNumber = 1;
    public float amountOfEnemiesIncreaseFactor;

    private float amountOfSpawnPoints = 1;
    private float amountOfEnemiesToSpawn = 1;

    private EnemySpawner enemyspawner;

    // Start is called before the first frame update
    void Start()
    {
        enemyspawner = GetComponent<EnemySpawner>();
    }

    public void SpawnWave()
    {
        //amountOfSpawnPoints = Mathf.Ceil(1f * Mathf.Pow(waveNumber, 2));
        amountOfEnemiesToSpawn = Mathf.Ceil(amountOfEnemiesIncreaseFactor * Mathf.Log(waveNumber * 5, 2));

        enemyspawner.SetSpawnPoints((int)amountOfSpawnPoints);
        //enemyspawner.SetSpawnPointsLocation();            // Used for setting of the random spawnpoints locations in the future
        enemyspawner.StartCoroutine(enemyspawner.SpawnEnemies((int)amountOfEnemiesToSpawn));

        waveNumber++;
    }
}
