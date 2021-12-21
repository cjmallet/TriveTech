using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private bool lost = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && !lost)
        {
            LevelManager.Instance.LoseCargo();
            lost = true;
        }
    }
}
