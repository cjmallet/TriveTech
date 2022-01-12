using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private int destructionSpeed;
    [SerializeField] private BoxCollider collideBox;
    private bool collided;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collided)
        {
            Rigidbody rb = collision.transform.GetComponentInParent<Rigidbody>();

            if (rb.velocity.magnitude>=destructionSpeed)
            {
                Instantiate(particles, transform.position, transform.rotation);
                Destroy(transform.gameObject);
                collided = !collided;
            }
            else
            {
                collideBox.enabled = true;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        collideBox.enabled = false;
    }
}
