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
        agent.destination = coreBlock.transform.position;
    }
}
