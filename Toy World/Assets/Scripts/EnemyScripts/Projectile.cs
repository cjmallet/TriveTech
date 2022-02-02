using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base for projectiles that are fired at the player.
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float removeTime;
    public int damage;
    public int speedModifier;
    private float timer;
    private bool firstObject=true;

    private void FixedUpdate()
    {
        //If no object has been touched keep the timer going
        if (!firstObject)
        {
            timer += Time.deltaTime;
        }

        //If the timer is higher than the despawn timer remove it from the scene
        if (timer >= removeTime)
        {
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// If the projectile has hit an object make it simlate a collision and deal damage
    /// to the first object it touches
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (firstObject)
        {
            transform.GetComponent<Rigidbody>().useGravity=true;
            firstObject = !firstObject;
        }
    }
}
