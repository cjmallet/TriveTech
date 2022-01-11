using System.Collections;
using UnityEngine;

public class WheelPart : MovementPart
{
    private Transform visualWheel;
    private Vector3 wheelPosition;
    private Quaternion wheelRotation;

    private void Start()
    {
        visualWheel = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (GetComponent<WheelCollider>().enabled)
            ApplyLocalPositionToVisuals();
    }

    public override void SwitchColliders()
    {
        GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
        GetComponent<WheelCollider>().enabled = !GetComponent<WheelCollider>().enabled;
    }

    public override void ApplyLocalPositionToVisuals()
    {
        if (transform.childCount == 0)
            return;

        GetComponent<WheelCollider>().GetWorldPose(out wheelPosition, out wheelRotation);

        visualWheel.transform.position = wheelPosition;
        visualWheel.transform.rotation = wheelRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("yes");
    }
}