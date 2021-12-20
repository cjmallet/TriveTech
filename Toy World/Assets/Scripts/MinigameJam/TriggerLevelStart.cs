using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevelStart : MonoBehaviour
{
    private bool levelStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (!levelStarted)
        {
            LevelManager.Instance.StartLevel();
            levelStarted = true;
        }
    }
}
