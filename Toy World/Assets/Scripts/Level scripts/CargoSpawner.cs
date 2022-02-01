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
        //If not all cargo has spawned and the timer has finished, spawn cargo
        if (spawningCargo && itemsSpawned < GameManager.Instance.levelManager.cargoToSpawn)
        {
            GameObject cargo = Instantiate(cargoPrefab, cargoSpawnPoint.transform.position, Quaternion.identity);
            cargo.transform.parent = cargoContainer.transform;
            spawningCargo = false;
            itemsSpawned++;
            GameManager.Instance.levelManager.collectedCargo++;

            StartCoroutine(SpawnTimer());
        }
        //Stop spawning cargo when all the cargo has spawned
        else if (itemsSpawned >= GameManager.Instance.levelManager.cargoToSpawn && !finishedSpawning)
        {
            spawningCargo = false;
            finishedSpawning = true;
            GameManager.Instance.levelManager.StartTimer();
        }
    }

    /// <summary>
    /// Keep the spawning of the cargo on a set timer
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(cargoSpawnSpeed);

        spawningCargo = true;
    }

    /// <summary>
    /// Start spawning the cargo
    /// </summary>
    public void SpawnItems()
    {
        spawningCargo = true;
        finishedSpawning = false;
        itemsSpawned = 0;

        if (cargoContainer == null)
            cargoContainer = new GameObject("Cargo container");
    }

    /// <summary>
    /// Remove the cargo from the scene
    /// </summary>
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

    /// <summary>
    /// Reset the settings for spawning the cargo
    /// </summary>
    public void ResetItems()
    {
        spawningCargo = false;
        finishedSpawning = true;
        itemsSpawned = GameManager.Instance.levelManager.cargoToSpawn;
        GameManager.Instance.levelManager.collectedCargo = 0;
    }
}