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

    /// <summary>
    /// Check if the player hits the object and his destructive power
    /// is great enough.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (!collided && collision.CompareTag("Part"))
        {
            Rigidbody rb = collision.transform.GetComponentInParent<Rigidbody>();

            //If the part colliding is not a spike and the vehicle is going faster than the destruction speed for no spikes
            if (collision.GetComponent<OffensivePart>() != null&&rb.velocity.magnitude>=lowDestructionSpeed)
            {
                HandleDestructionSound();

                //Apply pushback
                rb.velocity *= 0.9f;
                collision.GetComponent<Part>().TakeDamage(lowPartDamage, collision);
                Instantiate(particles, transform.position, transform.rotation);
                Destroy(transform.gameObject);

                collided = !collided;
            }
            //If the part colliding is a spike and the vehicle goes faster than the destruction speed for spikes
            else if (collision.GetComponent<OffensivePart>()==null &&rb.velocity.magnitude>= highDestructionSpeed)
            {
                HandleDestructionSound();

                //Apply pushback
                rb.velocity *= 0.5f;
                collision.GetComponent<Part>().TakeDamage(highPartDamage, collision);
                Instantiate(particles, transform.position, transform.rotation);
                Destroy(transform.gameObject);            

                collided = !collided;
            }
            //If the vehicle is not going fast enough to destroy the object
            else
            {
                //Enable the non trigger collider
                collideBox.enabled = true;
            }
        }
        if (collision.CompareTag("Missile"))
        {
            HandleDestructionSound();

            //Enable the non trigger collider
            collideBox.enabled = true;
        }
    }

    //If the player stops touching the object, disable the non trigger collider
    private void OnCollisionExit(Collision other)
    {
        collideBox.enabled = false;
    }

    /// <summary>
    /// Play the destruction sound effect when player destroys this object
    /// </summary>
    private void HandleDestructionSound()
    {
        GameObject audioSource = AudioManager.Instance.GetPooledAudioSourceObject();
        audioSource.transform.localPosition = gameObject.transform.position;
        audioSource.SetActive(true);

        AudioManager.Instance.Play(AudioManager.clips.BreakDestructibleObject, audioSource.GetComponent<AudioSource>());
    }  
}
