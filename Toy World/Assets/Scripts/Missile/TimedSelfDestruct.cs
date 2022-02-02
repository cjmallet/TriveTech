using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if object is ready to explode.
/// </summary>
public class TimedSelfDestruct : MonoBehaviour
{
    public float maxLifeSpan;
    private float _timeAlive;



    // Update is called once per frame
    void FixedUpdate()
    {
        _timeAlive += Time.deltaTime;
        if (_timeAlive >= maxLifeSpan)
            Destroy(gameObject);
    }
}
