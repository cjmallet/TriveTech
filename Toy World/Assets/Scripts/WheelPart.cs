using UnityEngine;

public class WheelPart : MovementPart
{
    public float steeringAngle { get; set; }
    private const int STEERINGMAX = 90;

    public WheelPart()
    {
        this.steeringAngle = 0;
    }

    public override void ForwardAction(float moveAmount)
    {
        if (moveAmount > 0)
        {
            transform.GetChild(1).Rotate(0, moveAmount, 0);
        }
    }

    public override void BackwardAction(float moveAmount)
    {
        if (moveAmount > 0)
        {
            transform.GetChild(1).Rotate(0, -moveAmount, 0);
        }
    }

    public override void RightAction(float turnAmount = 0)
    {
        Debug.Log(transform.localEulerAngles.y);
        if (turnAmount > 0 && (transform.localEulerAngles.y <= STEERINGMAX))
        {
            //steeringAngle += turnAmount;

            transform.Rotate(0, turnAmount, 0);
        }
    }

    public override void LeftAction(float turnAmount = 0)
    {
        Debug.Log(transform.localEulerAngles.y);
        if (turnAmount < 0 && (transform.localEulerAngles.y >= (360 - STEERINGMAX)))
        {
            //steeringAngle -= turnAmount;

            transform.Rotate(0, turnAmount, 0);
        }
    }
}
