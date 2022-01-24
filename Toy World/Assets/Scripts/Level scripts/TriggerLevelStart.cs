using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevelStart : MonoBehaviour
{
    public bool levelStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (!levelStarted && other.CompareTag("CoreBlock"))
        {
            GameManager.Instance.levelManager.StartLevel();
            levelStarted = true;
        }
    }
}
