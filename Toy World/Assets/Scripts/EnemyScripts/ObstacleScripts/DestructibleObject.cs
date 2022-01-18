using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private int highDestructionSpeed,lowDestructionSpeed;
    [SerializeField] private int highPartDamage, lowPartDamage;
    [SerializeField] private BoxCollider collideBox;
    private bool collided;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collided && collision.CompareTag("CoreBlock"))
        {
            Rigidbody rb = collision.transform.GetComponentInParent<Rigidbody>();

            if (collision.GetComponent<OffensivePart>() != null&&rb.velocity.magnitude>=lowDestructionSpeed)
            {
                rb.velocity *= 0.9f;
                collision.GetComponent<Part>().TakeDamage(lowPartDamage, collision);
                Instantiate(particles, transform.position, transform.rotation);
                Destroy(transform.gameObject);

                collided = !collided;
            }
            else if (collision.GetComponent<OffensivePart>()==null &&rb.velocity.magnitude>= highDestructionSpeed)
            {
                rb.velocity *= 0.5f;
                collision.GetComponent<Part>().TakeDamage(highPartDamage,collision);
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
