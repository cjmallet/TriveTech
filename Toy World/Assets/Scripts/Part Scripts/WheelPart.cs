using System.Collections;
using UnityEngine;

/// <summary>
/// The wheel part that apply's visuals to a wheel collider.
/// </summary>
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

    /// <summary>
    /// Translates the visual wheel to wheelcollider position.
    /// </summary>
    public override void ApplyLocalPositionToVisuals()
    {
        if (transform.childCount == 0)
            return;

        GetComponent<WheelCollider>().GetWorldPose(out wheelPosition, out wheelRotation);

        visualWheel.transform.position = wheelPosition;
        visualWheel.transform.rotation = wheelRotation;
    }
}