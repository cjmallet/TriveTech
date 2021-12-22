using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevelStart : MonoBehaviour
{
    public bool levelStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (!levelStarted && other.name == "CoreBlock")
        {
            LevelManager.Instance.StartLevel();
            levelStarted = true;
        }
    }
}
