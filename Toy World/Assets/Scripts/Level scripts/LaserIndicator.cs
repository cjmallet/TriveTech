using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a laser from the cargo drop point to indicate where the cargo will fall
/// </summary>
public class LaserIndicator : MonoBehaviour
{
    private LineRenderer lrLaser;
    private GameObject laserImpactEffect;

    private void Start()
    {
        lrLaser = GetComponent<LineRenderer>();
        laserImpactEffect = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        ShowLaser();
    }

    /// <summary>
    /// Draws the laser and its impact effects using a raycast
    /// </summary>
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
