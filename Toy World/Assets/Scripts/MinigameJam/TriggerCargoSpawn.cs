using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCargoSpawn : MonoBehaviour
{
    private SpawnCargo cargoSpawner;
    private bool cargoSpawned;
    // Start is called before the first frame update
    void Start()
    {
        cargoSpawner = transform.parent.GetComponent<SpawnCargo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cargoSpawned)
        {
            cargoSpawner.SpawnItems();
            cargoSpawned = true;
        }
    }
}
