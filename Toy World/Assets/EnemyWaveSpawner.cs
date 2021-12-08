using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    public int waveNumber = 1;
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

        //amountOfSpawnPoints = Mathf.Ceil(0.1f * Mathf.Pow(waveNumber, 2)) * 10;
        //amountOfEnemiesToSpawn = Mathf.Ceil(0.3f * Mathf.Pow(waveNumber, 2)) * 10;

        enemyspawner.SetSpawnPoints((int)amountOfSpawnPoints);
        enemyspawner.SetSpawnPointsLocation();
        enemyspawner.StartCoroutine(enemyspawner.SpawnEnemies((int)amountOfEnemiesToSpawn));

        waveNumber++;
    }
}
