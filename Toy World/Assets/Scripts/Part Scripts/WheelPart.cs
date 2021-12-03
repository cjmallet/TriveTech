using System.Collections;
using UnityEngine;

public class WheelPart : MovementPart
{
    private int moveSpeed = 10;
    private int rotationSpeed = 10;
    private float myMass = 0.1f;

    public float steeringAngle { get; set; }
    private const int STEERINGMAX = 30;
    private const float CENTERSPEED = 2f;
    private bool turning = false, moving = false;

    private bool CheckIfTurn { get { return (transform.localEulerAngles.y >= 0 && (int)transform.localEulerAngles.y < STEERINGMAX)
            || ((int)transform.localEulerAngles.y > 360 - STEERINGMAX && transform.localEulerAngles.y <= 360); } }

    public WheelPart()
    {
        this.moveSpeedModifier = moveSpeed;
        this.rotationSpeedModifier = rotationSpeed;
        this.Weight = myMass;
    }

    public override bool IsGrounded()
    {
        return base.IsGrounded();
    }

    public override void VerticalMovement(float moveAmount)
    {
        //Debug.Log(moveAmount);

        /*if (moveAmount > 0) // forward
        {
            transform.GetChild(1).Rotate(0, -moveAmount * rotationSpeed, 0);
            moving = true;
            CenterWheel(CENTERSPEED);
        }
        else if (moveAmount < 0) // backward
        {
            transform.GetChild(1).Rotate(0, moveAmount * rotationSpeed, 0);
            moving = true;
            CenterWheel(CENTERSPEED);
        }*/
    }

    private IEnumerator TurnWheel(float turnAmount)
    {
        if (turnAmount > 0 && CheckIfTurn) // right
        {
            transform.Rotate(0, turnAmount * Time.deltaTime, 0);
        }
        else if (turnAmount < 0 && CheckIfTurn) // left
        {
            transform.Rotate(0, turnAmount * Time.deltaTime, 0);
        }
        else
        {
            if ((int)transform.localEulerAngles.y == STEERINGMAX && turnAmount < 0) // Can only go left
                transform.Rotate(0, turnAmount * Time.deltaTime, 0);
            else if ((int)transform.localEulerAngles.y == 360 - STEERINGMAX && turnAmount > 0) // Can only go right
                transform.Rotate(0, turnAmount * Time.deltaTime, 0);
            else if ((int)transform.localEulerAngles.y != STEERINGMAX && (int)transform.localEulerAngles.y != 360 - STEERINGMAX && turning)
                transform.Rotate(0, turnAmount * Time.deltaTime, 0);
        }

        yield return new WaitForEndOfFrame();

        if (turning)
            yield return StartCoroutine(TurnWheel(turnAmount));
    }

    public override void HorizontalMovement(float turnAmount)
    {
        // left is -1, right is 1
        if (turnAmount != 0)
        {
            turning = true;
            StartCoroutine(TurnWheel(turnAmount));
        }          
    }

    public override void NoTurning()
    {
        turning = false;
    }

    public override void NoMoving()
    {
        moving = false;
    }

    /*private void CenterWheel(float CenterSpeed)
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
        
    }*/
}