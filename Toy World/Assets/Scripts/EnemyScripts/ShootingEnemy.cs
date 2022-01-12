using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackInterval;
    private GameObject projectileSpawnPoint;
    private GameObject target;
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

        if (timer > attackInterval)
        {
            GameObject spawnedProjectile= Instantiate(projectile, projectileSpawnPoint.transform.position,projectileSpawnPoint.transform.rotation);
            Vector3 direction = (target.transform.position- spawnedProjectile.transform.position).normalized;
            spawnedProjectile.GetComponent<Rigidbody>().AddForce(direction*speedModifier);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
        target = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        timer = 0;
    }
}
