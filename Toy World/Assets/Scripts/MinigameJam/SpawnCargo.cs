using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnCargo : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab,cargoSpawnPoint, timerObject;
    [SerializeField] private int cargoToSpawn;
    [SerializeField] private int timeLevelCompletion;

    private float waitTime = 0.2f,timer,itemsSpawned;
    private bool underSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (underSpawn)
        {
            timer += Time.deltaTime;
            timerObject.GetComponent<TextMeshProUGUI>().text = ((int)(timeLevelCompletion-timer)).ToString();
        }
        
        if (underSpawn && timer>waitTime &&itemsSpawned<cargoToSpawn)
        {
            timer = 0;
            itemsSpawned++;
            Instantiate(cargoPrefab, cargoSpawnPoint.transform);
        }

        if (timer>timeLevelCompletion)
        {
            Debug.Log("Failed");
        }
    }

    public void SpawnItems()
    {
        underSpawn = true;
    }
}
