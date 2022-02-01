using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    /// <summary>
    /// If the player collides with this object, end the level
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("CoreBlock"))
            GameManager.Instance.levelManager.FinishLevel();
    }
}
