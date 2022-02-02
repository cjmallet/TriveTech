using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The gameobject that detects when the player has 'ended' the level.
/// </summary>
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
