using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCollider : MonoBehaviour
{
    [HideInInspector] public string collisionName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionName = collision.transform.name;
    }
}
