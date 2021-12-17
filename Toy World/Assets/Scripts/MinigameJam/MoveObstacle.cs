using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public bool horizontal, vertical;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (horizontal && !vertical)
            transform.position = startPos + new Vector3(Mathf.Sin(Time.time) * 10f, 0, 0);
        if (vertical && !horizontal)
            transform.position = startPos + new Vector3(0, 0, Mathf.Sin(Time.time) * 10f);

        if (horizontal && vertical)
            transform.position = startPos + new Vector3(Mathf.Sin(Time.time) * 10f, 0, Mathf.Sin(Time.time) * 10f);
    }
}
