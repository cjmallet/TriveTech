using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            Instantiate(enemy,spawnPoint.transform);
        }   
    }
}
