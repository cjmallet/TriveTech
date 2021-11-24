using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : MovementPart
{
    public int steeringAngle { get; set; }
    private const int STEERINGMAX = 90;


    public WheelPart()
    {
        this.steeringAngle = 0;
    }

    public override void Forward(int moveAmount)
    {
        while (moveAmount > 0)
        {
            transform.GetChild(1).Rotate(new Vector3(moveAmount, 0, 0));
        }
    }

    public override void Turn(int turnAmount = 0)
    {
        while (turnAmount > 0 && (steeringAngle < STEERINGMAX || steeringAngle > -STEERINGMAX))
        {
            steeringAngle = turnAmount;

            transform.Rotate(new Vector3(0, steeringAngle, 0));
        }
    }
}
