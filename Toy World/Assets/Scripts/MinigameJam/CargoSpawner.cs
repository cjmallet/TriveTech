using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CargoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab, cargoSpawnPoint;
    [SerializeField] private float cargoSpawnSpeed;

    private GameObject cargoContainer;

    private float itemsSpawned = 0;
    private bool spawningCargo, finishedSpawning = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawningCargo && itemsSpawned < LevelManager.Instance.cargoToSpawn)
        {
            GameObject cargo = Instantiate(cargoPrefab, cargoSpawnPoint.transform.position, Quaternion.identity);
            cargo.transform.parent = cargoContainer.transform;
            spawningCargo = false;
            itemsSpawned++;
            LevelManager.Instance.collectedCargo++;

            StartCoroutine(SpawnTimer());
        }
        else if (itemsSpawned >= LevelManager.Instance.cargoToSpawn && !finishedSpawning)
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
        finishedSpawning = false;
        itemsSpawned = 0;

        if (cargoContainer == null)
            cargoContainer = new GameObject("Cargo container");
    }

    public void CleanCargo()
    {
        if (cargoContainer != null)
        {
            foreach (Transform child in cargoContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ResetItems()
    {
        spawningCargo = false;
        finishedSpawning = true;
        itemsSpawned = LevelManager.Instance.cargoToSpawn;
        LevelManager.Instance.collectedCargo = 0;
    }
}