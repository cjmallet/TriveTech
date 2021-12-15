using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePart : Part
{
    private List<Collider> colliders = new List<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (!colliders.Contains(collision.GetContact(i).thisCollider))
            {
                colliders.Add(collision.GetContact(i).thisCollider);
                collision.GetContact(i).thisCollider.gameObject.GetComponent<Part>().HandleCollision(collision.GetContact(i).otherCollider);
            }
        }

        colliders.Clear();

        if (collision.gameObject.name.Contains("StartWavesTestBlock") && GameObject.Find("BoopBlueBlock").gameObject.activeSelf)
        {
            Debug.Log("test");
            EnemyWaveSpawner enemyWaveSpawner = GameObject.Find("EnemyManager").GetComponent<EnemyWaveSpawner>();
            enemyWaveSpawner.SpawnWave();
        }
    }
}
