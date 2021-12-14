using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject coreBlock;

    public GameObject explosion;

    public int damage = 1;

    public int agentId;

    public float separationWeight;
    public float cohesionWeight;
    public float alignmentWeight;
    public float minDistanceRadius;

    private bool engaged = false;

    // Start is called before the first frame update
    void Start()
    {
        coreBlock = GameObject.Find("CoreBlock");
        agent = GetComponent<NavMeshAgent>();
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

    private Vector3 FlockingBehaviour()
    {
        Vector3 cohesionVector = new Vector3();
        Vector3 separateVector = new Vector3();
        Vector3 forwardVector = new Vector3();

        int count = 0;

        for (int i = 0; i < EnemySpawner.enemyList.Count; i++)
        {
            if (agentId != EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().agentId)
            {
                float distance = (transform.position - EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position).sqrMagnitude;

                if (distance > 0 && distance < minDistanceRadius)
                {
                    cohesionVector += EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position;
                    separateVector += EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position - transform.position;
                    forwardVector += EnemySpawner.enemyList[i].GetComponent<NavMeshAgentBehaviour>().transform.position;

                    count++;
                }
            }
        }

        if (count == 0)
        {
            return Vector3.zero;
        }

        // revert vector
        // separation step
        separateVector /= count;
        separateVector *= -1;

        // forward step
        forwardVector /= count;

        // cohesione step
        cohesionVector /= count;
        cohesionVector = (cohesionVector - transform.position);

        Vector3 flockingVector = ((separateVector.normalized * separationWeight) +
                                 (cohesionVector.normalized * cohesionWeight) +
                                 (forwardVector.normalized * alignmentWeight));

        return flockingVector;

    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}

        

