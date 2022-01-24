using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [HideInInspector]public bool lost = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && !lost)
        {
            GameManager.Instance.levelManager.LoseCargo();
            lost = true;
        }
    }
}
