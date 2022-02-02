using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of cargo.
/// </summary>
public class Wood : MonoBehaviour
{
    [HideInInspector]public bool lost = false;

    /// <summary>
    /// If the cargo touches the ground for the first time
    /// Remove it from the main objective counter in the level manager
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && !lost)
        {
            GameManager.Instance.levelManager.LoseCargo();
            lost = true;
        }
    }
}
