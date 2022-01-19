using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementPart : Part
{
    public bool frontPart { get; set; }

    public float maxTorgue;

    public float steeringAngle;

    public virtual void ApplyLocalPositionToVisuals()
    {

    }
}