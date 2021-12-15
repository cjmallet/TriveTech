using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject coreBlock;

    public GameObject explosion;

    public int damage = 3;

    public int agentId;

    public float size;
    public float speed;

    //public float separationWeight;
    //public float cohesionWeight;
    //public float alignmentWeight;
    //public float minDistanceRadius;

    private bool engaged = false;

    private EnemyWaveSpawner waveSpawner;

    // Start is called before the first frame update
    void Start()
    {
        waveSpawner = GameObject.Find("EnemyManager").GetComponent<EnemyWaveSpawner>();
        coreBlock = GameObject.Find("CoreBlock");

        agent = GetComponent<NavMeshAgent>();

        size = Random.Range(0.5f, 5f);
        damage *= (int)size;

        if (size < 1.5f)
        {
            speed = Random.Range(8f, 10f);
        }
        else if (size < 2.2f)
        {
            speed = Random.Range(6.5f, 8f);
        }
        else
        {
            speed = Random.Range(5, 6.5f);
        }

        if (waveSpawner.waveNumber - 1 == 2 || waveSpawner.waveNumber - 1 == 4)
        {
            speed *= 5f;
            damage *= 5;
            Debug.Log(damage);
        }
        else
        {
            speed *= 2.5f;
            Debug.Log(damage);
        }

        gameObject.transform.localScale = new Vector3(size * 0.5f, size * 1.4f, size * 0.5f);
        agent.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        Activate();
    }

    private void Activate()
    {
        //agent.velocity += FlockingBehaviour();
        //transform.position += agent.velocity * Time.deltaTime;
        //transform.forward = agent.velocity.normalized;

        if (Vector3.Distance(transform.position, coreBlock.transform.position) < EnemySpawner.distanceToActivateValue || engaged)
        {
            //agent.velocity += FlockingBehaviour();
            agent.destination = coreBlock.transform.position;
            engaged = true;
        }
        else if (!engaged)
        {
            //agent.velocity += FlockingBehaviour();
            agent.destination = this.transform.position - transform.forward * 0.01f;
        }
    }

    private void OnDestroy()
    {
        EnemySpawner.enemyList.Remove(gameObject);
    }

    //private Vector3 FlockingBehaviour()
    //{
    //    Vector3 cohesionVector = new Vector3();
    //    Vector3 separateVector = new Vector3();
    //    Vector3 forwardVector = new Vector3();

    //    int count = 0;

    //    for (int i = 0; i < EnemySpawner.enemyList.Count; i++)
    //    {
    //        if (agentId != EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().agentId)
    //        {
    //            float distance = (transform.position - EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position).sqrMagnitude;

    //            if (distance > 0 && distance < minDistanceRadius)
    //            {
    //                cohesionVector += EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position;
    //                separateVector += EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position - transform.position;
    //                forwardVector += EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position;

    //                count++;
    //            }
    //        }
    //    }

    //    if (count == 0)
    //    {
    //        return Vector3.zero;
    //    }

    //    // revert vector
    //    // separation step
    //    separateVector /= count;
    //    separateVector *= -1;

    //    // forward step
    //    forwardVector /= count;

    //    // cohesione step
    //    cohesionVector /= count;
    //    cohesionVector = (cohesionVector - transform.position);

    //    Vector3 flockingVector = ((separateVector.normalized * separationWeight) +
    //                             (cohesionVector.normalized * cohesionWeight) +
    //                             (forwardVector.normalized * alignmentWeight));

    //    return flockingVector;

    //}

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}

        

