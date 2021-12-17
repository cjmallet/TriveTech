using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CargoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab, cargoSpawnPoint;
    [SerializeField] private int cargoToSpawn;
    [SerializeField] private float cargoSpawnSpeed;

    private float itemsSpawned = 0;
    private bool  spawningCargo, finishedSpawning = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawningCargo && itemsSpawned < cargoToSpawn)
        {
            Instantiate(cargoPrefab, cargoSpawnPoint.transform.position, Quaternion.identity);
            spawningCargo = false;
            itemsSpawned++;
            LevelManager.Instance.collectedCargo++;

            StartCoroutine(SpawnTimer());
        }
        else if (itemsSpawned >= cargoToSpawn && !finishedSpawning)
        {
            spawningCargo = false;
            finishedSpawning = true;
            LevelManager.Instance.StartTimer();
        }
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(cargoSpawnSpeed);

        spawningCargo = true;
    }

    public void SpawnItems()
    {
        spawningCargo = true;
    }
}