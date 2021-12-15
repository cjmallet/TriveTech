using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private SpawnCargo levelmanager;

    private void OnTriggerEnter(Collider other)
    {
        levelmanager.Finish();
    }
}
