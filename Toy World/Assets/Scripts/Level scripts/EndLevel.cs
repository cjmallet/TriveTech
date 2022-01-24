using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.name.Contains("Wood"))
            GameManager.Instance.levelManager.FinishLevel();
    }
}
