using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyWaveSpawner : MonoBehaviour
{
    public int waveNumber = 0;
    public float amountOfEnemiesIncreaseFactor;

    private float amountOfSpawnPoints = 1;
    private float amountOfEnemiesToSpawn = 1;

    private EnemySpawner enemyspawner;

    //public GameObject enemyPrefab;

    public TextMeshProUGUI boopBlueBlockToStartWave;
    public TextMeshProUGUI pressP;
    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI runAway;
    public TextMeshProUGUI killAllEnemies;
    public TextMeshProUGUI youWon;

    public float chaseTimerValue = 20f;

    // Start is called before the first frame update
    void Start()
    {
        enemyspawner = GetComponent<EnemySpawner>();
        // waveCounterText = GameObject.Find("WaveCounter").GetComponent<Text>();
    }

    public void SpawnWave()
    {
        chaseTimerValue = 20f;

        boopBlueBlockToStartWave.gameObject.SetActive(false);
        pressP.gameObject.SetActive(false);

        //amountOfSpawnPoints = Mathf.Ceil(1f * Mathf.Pow(waveNumber, 2));
        amountOfEnemiesToSpawn = Mathf.Ceil(amountOfEnemiesIncreaseFactor * Mathf.Log(waveNumber * 5, 2));

        enemyspawner.SetSpawnPoints((int)amountOfSpawnPoints);
        //enemyspawner.SetSpawnPointsLocation();            // Used for setting of the random spawnpoints locations in the future
        enemyspawner.StartCoroutine(enemyspawner.SpawnEnemies((int)amountOfEnemiesToSpawn));

        waveCounterText.text = "Wave " + waveNumber + " / 5";

        waveNumber++;
    }

    private void Update()
    {
        if (waveNumber - 1 == 2 || waveNumber - 1 == 4)
        {
            runAway.gameObject.SetActive(true);
            TimerText.gameObject.SetActive(true);

            chaseTimerValue -= Time.deltaTime;

            TimerText.text = "Time: " + (int)chaseTimerValue;

            if (chaseTimerValue <= 0)
            {
                foreach (Transform enemy in GameObject.Find("EnemyParent").transform)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }
        else if(waveNumber + 1 == 1 || waveNumber + 1 == 3 || waveNumber + 1 == 5)
        {
            killAllEnemies.gameObject.SetActive(true);
        }
        

        if (GameObject.Find("EnemyParent").transform.childCount == 0)
        {
            if (GameObject.Find("VehicleEditor").GetComponent<VehicleEditor>().playan)
            {
                boopBlueBlockToStartWave.gameObject.SetActive(true);
            }
            
            runAway.gameObject.SetActive(false);
            killAllEnemies.gameObject.SetActive(false);
            TimerText.gameObject.SetActive(false);

            if (waveNumber != 1)
            {
                pressP.gameObject.SetActive(true);
            }

            if (waveNumber - 1 == 5)
            {
                youWon.gameObject.SetActive(true);
                GameObject.Find("StartWavesTestBlock").gameObject.SetActive(false);
            }
        }
    }
}
