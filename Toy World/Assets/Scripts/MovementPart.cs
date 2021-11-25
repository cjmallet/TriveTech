using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPart : Part
{
    [HideInInspector] public VehicleMovement vehicleMovement;

    public MovementPart()
    {
        
    }

    public virtual void Start()
    {
        vehicleMovement = transform.parent.GetComponent<VehicleMovement>();
    }

    public virtual void Forward(int moveAmount)
    {
        
    }

    public virtual void Turn(int turnAmount)
    {

    }
}
