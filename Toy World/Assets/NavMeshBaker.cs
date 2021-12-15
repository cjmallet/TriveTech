using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        NavMeshSurface navSurface = gameObject.GetComponentInChildren<NavMeshSurface>();
        navSurface.BuildNavMesh();
    }      
}
