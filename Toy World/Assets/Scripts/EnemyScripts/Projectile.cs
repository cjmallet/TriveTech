using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float removeTime;
    public int damage;
    public int speedModifier;
    private float timer;
    private bool firstObject=true;

    private void FixedUpdate()
    {
        if (!firstObject)
        {
            timer += Time.deltaTime;
        }

        if (timer >= removeTime)
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (firstObject)
        {
            transform.GetComponent<Rigidbody>().useGravity=true;
            firstObject = !firstObject;
        }
    }
}
