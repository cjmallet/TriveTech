using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public bool horizontal, vertical, startSide; // startSide true = left to right; startSide false = right to left 
    public float speed;
    public int damage;
    public float distance;

    private Vector3 startPos;
    private float t = 0.0f;

    private bool active;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (active && !horizontal && !vertical)
        {
            if (startSide)
            {
                transform.position = new Vector3(Mathf.Lerp(startPos.x, startPos.x + distance, t), startPos.y, startPos.z);
            }
            else if (!startSide)
            {
                transform.position = new Vector3(Mathf.Lerp(startPos.x, startPos.x - distance, t), startPos.y, startPos.z);
            }

            t += 0.5f * Time.deltaTime;
        }
        //else if (active && (horizontal || vertical))
        //{
        //    if (horizontal && !vertical)
        //        transform.position = startPos + new Vector3(Mathf.Sin(Time.time) * speed, 0, 0);
        //    if (vertical && !horizontal)
        //        transform.position = startPos + new Vector3(0, 0, Mathf.Sin(Time.time) * speed);

        //    if (horizontal && vertical)
        //        transform.position = startPos + new Vector3(Mathf.Sin(Time.time) * speed, 0, Mathf.Sin(Time.time) * speed);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("CoreBlock"))
        {
            active = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("CoreBlock") || collision.gameObject.name.Contains("part") || collision.gameObject.name.Contains("Spike") || collision.gameObject.name.Contains("Butt"))
        {
            StartCoroutine(IAmWalkingHere());
        }
    }

    private IEnumerator IAmWalkingHere()
    {
        yield return new WaitForSeconds(0.1f);
        active = false;
    }
}
