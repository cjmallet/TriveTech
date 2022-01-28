using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserIndicator : MonoBehaviour
{
    private LineRenderer lrLaser;
    private GameObject laserImpactEffect;

    // Start is called before the first frame update
    private void Start()
    {
        lrLaser = GetComponent<LineRenderer>();
        laserImpactEffect = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowLaser();
    }

    private void ShowLaser()
    {
        lrLaser.SetPosition(0, transform.position + transform.up);

        RaycastHit laserHit;
        if (Physics.Raycast(transform.position, -transform.up, out laserHit))
        {
            lrLaser.SetPosition(1, laserHit.point);
            laserImpactEffect.transform.position = laserHit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position - transform.up * 10);
    }
}
