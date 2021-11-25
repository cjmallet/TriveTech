using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : MovementPart
{
    public int steeringAngle { get; set; }
    private const int STEERINGMAX = 90;

    public WheelPart()
    {
        this.steeringAngle = 2;
    }

    public override void Start()
    {
        base.Start();
        vehicleMovement.steeringAngle = steeringAngle;
    }

    private void Update()
    {
        Turn(45);
    }

    public override void Forward(int moveAmount)
    {
        while (moveAmount > 0)
        {
            transform.GetChild(1).Rotate(moveAmount, 0, 0);
        }
    }

    public override void Turn(int turnAmount)
    {
        if (vehicleMovement.moveLeft)
        {
            transform.Rotate(0, -turnAmount, 0);
            //transform.rotation = new Quaternion(transform.rotation.x, transform.localRotation.y - 45, transform.rotation.z, transform.rotation.w);
        }

        if (vehicleMovement.moveRight)
        {
            transform.Rotate(0, turnAmount, 0);
            //transform.rotation = new Quaternion(transform.rotation.x, transform.localRotation.y+45, transform.rotation.z, transform.rotation.w);
        }

        /*
        while (turnAmount > 0 && (steeringAngle < STEERINGMAX || steeringAngle > -STEERINGMAX))
        {
            steeringAngle = turnAmount;

            transform.Rotate(0, steeringAngle, 0);
        }
        */
    }
}
