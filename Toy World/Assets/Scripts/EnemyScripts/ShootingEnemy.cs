using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackInterval;
    private GameObject projectileSpawnPoint;
    private GameObject target;
    private List<GameObject> cargoInRange = new List<GameObject>();
    private float timer;
    private float speedModifier;
    private bool playerInRange;

    private void Start()
    {
        projectileSpawnPoint = transform.GetChild(0).gameObject;
        speedModifier=projectile.GetComponent<Projectile>().speedModifier;
    }

    private void FixedUpdate()
    {
        if (playerInRange)
        {
            timer += Time.deltaTime;
        }

        //Spawn a projectile when the timer is higher than the attack interval
        if (timer >= attackInterval)
        {
            GameObject spawnedProjectile= Instantiate(projectile, projectileSpawnPoint.transform.position,projectileSpawnPoint.transform.rotation);
            Vector3 direction = (target.transform.position- spawnedProjectile.transform.position).normalized;
            spawnedProjectile.GetComponent<Rigidbody>().AddForce(direction*speedModifier*100);
            timer = 0;
        }
    }

    /// <summary>
    /// Check if a cargo enters its trigger.
    /// If it is not lost yet, target that cargo.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Cargo")&& !other.GetComponent<Wood>().lost)
        {
            playerInRange = true;
            target = other.gameObject;
            cargoInRange.Add(other.gameObject);
        }
    }

    /// <summary>
    /// Check if a cargo is in its trigger and if it is not lost
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Cargo") && !other.GetComponent<Wood>().lost && cargoInRange.Count != 0)
        {
            target = other.gameObject;
        }

        if(other.name.Contains("Cargo") && other.GetComponent<Wood>().lost && cargoInRange.Count != 0)
        {
            cargoInRange.Remove(other.gameObject);
        }

        if (cargoInRange.Count == 0)
        {
            playerInRange = false;
        }
    }

    /// <summary>
    /// Remove any cargo from its sight if it gets out of range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Cargo"))
        {
            playerInRange = false;
            timer = 0;
            cargoInRange.Remove(other.gameObject);
        }
    }
}
