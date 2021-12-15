using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxDistance;

    private Rigidbody enemyRB;
    private bool travelDirection;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.z<maxDistance+maxDistance&&travelDirection)
        {
            enemyRB.AddForce(Vector3.forward*movementSpeed);
        }

        if(transform.localPosition.z >-maxDistance&&!travelDirection)
        {
            enemyRB.AddForce(Vector3.forward * -movementSpeed);
        }

        if (transform.localPosition.z > maxDistance)
        {
            travelDirection = false;
        }

        if (transform.localPosition.z < - maxDistance)
        {
            travelDirection = true;
        }
    }
}
