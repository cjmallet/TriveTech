using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject coreBlock;

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
        if (Vector3.Distance(transform.position, coreBlock.transform.position) < EnemySpawner.distanceToActivateValue)
        {
            agent.destination = coreBlock.transform.position;
        }
        else
        {
            agent.destination = this.transform.position - transform.forward * 0.01f;
        }
    }
}
