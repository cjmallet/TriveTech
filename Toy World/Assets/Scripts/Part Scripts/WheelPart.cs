using UnityEngine;

public class WheelPart : MovementPart
{
    private int speed = 5;

    public WheelPart()
    {
        this.speedModifier = speed;
    }

    /*
    public float steeringAngle { get; set; }
    private const int STEERINGMAX = 30;
    private const float CENTERSPEED = 2f;
    private bool turning = false;
    [SerializeField]
    private int rotationSpeed;

    private bool CheckIfRight { get { return transform.localEulerAngles.y >= 0 && transform.localEulerAngles.y <= STEERINGMAX + 1; } }
    private bool CheckIfLeft { get { return transform.localEulerAngles.y >= 330 && transform.localEulerAngles.y <= 360; } }

    public WheelPart()
    {
        this.steeringAngle = 0;
    }

    public override void ForwardAction(float moveAmount)
    {
        if (moveAmount > 0)
        {
            transform.GetChild(1).Rotate(0, -moveAmount * rotationSpeed, 0);
            CenterWheel(CENTERSPEED);
        }
    }

    public override void BackwardAction(float moveAmount)
    {
        if (moveAmount > 0)
        {
            transform.GetChild(1).Rotate(0, moveAmount * rotationSpeed, 0);
            CenterWheel(CENTERSPEED);
        }
    }

    private void CenterWheel(float CenterSpeed)
    {
        if (!turning && transform.localEulerAngles.y != 0 && (CheckIfRight))
        {
            transform.Rotate(0, -CenterSpeed, 0);
        }
        else if (!turning && transform.localEulerAngles.y != 0 && (CheckIfLeft))
        {
            transform.Rotate(0, CenterSpeed, 0);
        }

        if (transform.localEulerAngles.y > -0.05f && transform.localEulerAngles.y < 0.05f)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        
    }

    public override void RightAction(float turnAmount = 0)
    {
        if (turnAmount > 0 && (transform.localEulerAngles.y < STEERINGMAX || (CheckIfLeft)))
        {
            transform.Rotate(0, turnAmount, 0);
            turning = true;
        }
        else if (!(transform.localEulerAngles.y >= 0 && transform.localEulerAngles.y <= STEERINGMAX))
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, STEERINGMAX, transform.localEulerAngles.z);
    }

    public override void LeftAction(float turnAmount = 0)
    {
        if (-turnAmount < 0 && (transform.localEulerAngles.y > 360 - STEERINGMAX || (CheckIfRight)))
        {
            transform.Rotate(0, -turnAmount, 0);
            turning = true;
        }
        else if (transform.localEulerAngles.y < 360 - STEERINGMAX && transform.localEulerAngles.y > STEERINGMAX)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 360 - STEERINGMAX, transform.localEulerAngles.z);
    }

    public override void StopAction(bool stopped)
    {
        turning = stopped;
    }
    */ // Old system
}