using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private int destructionSpeed;
    [SerializeField] private BoxCollider collideBox;
    private bool collided;

    private float CalculateSpeed(float directionalSpeed)
    {
        if (directionalSpeed<0)
        {
            return -directionalSpeed;
        }
        else
        {
            return directionalSpeed;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collided)
        {
            Rigidbody rb = collision.transform.GetComponentInParent<Rigidbody>();
            float speed = CalculateSpeed(rb.velocity.x) + CalculateSpeed(rb.velocity.y) + CalculateSpeed(rb.velocity.z);

            if (speed>=destructionSpeed)
            {
                Instantiate(particles, transform.position, transform.rotation);
                Destroy(transform.gameObject);
                collided = !collided;
            }
            else
            {
                collideBox.enabled = true;
            }
            Debug.Log(speed+" "+ rb.velocity.x+" "+rb.velocity.y+" "+rb.velocity.z);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        collideBox.enabled = false;
    }
}
