using System.Collections;
using UnityEngine;

public class WheelPart : MovementPart
{
    private int moveSpeed = 15;
    private int rotationSpeed = 5;
    private float myMass = 0.1f;
    private int myHealth = 5;

    private float vertSpeed, horSpeed;

    public float steeringAngle { get; set; }
    private const int STEERINGMAX = 30;
    private bool turning = false, moving = false;

    private bool CheckIfTurn { get { return (transform.localEulerAngles.y >= 0 && (int)transform.localEulerAngles.y < STEERINGMAX)
            || ((int)transform.localEulerAngles.y > 360 - STEERINGMAX && transform.localEulerAngles.y <= 360); } }

    public WheelPart()
    {
        this.moveSpeedModifier = moveSpeed;
        this.rotationSpeedModifier = rotationSpeed;
        this.Weight = myMass;
        this.Health = myHealth;
    }

    public override bool IsGrounded()
    {
        return base.IsGrounded();
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            MoveWheel(vertSpeed);
        }

        if (turning)
        {
            TurnWheel(horSpeed);
        }
    }

    public override void VerticalMovement(float moveAmount)
    {
        if (moveAmount != 0)
        {
            vertSpeed = moveAmount;
            moving = true;
        }
    }

    private void MoveWheel(float moveAmount)
    {
        transform.GetChild(1).Rotate(0, -moveAmount * Time.deltaTime * (moveSpeed * 2), 0);

        if (transform.localEulerAngles.y != 0 && moving && !turning)
            if (moveAmount > 0)
                CenterWheel(moveAmount);
            else if (moveAmount < 0)
                CenterWheel(-moveAmount);
    }

    public override void HorizontalMovement(float turnAmount)
    {
        if (turnAmount != 0)
        {
            horSpeed = turnAmount;
            turning = true;
        }          
    }

    private void TurnWheel(float turnAmount)
    {
        if (turnAmount > 0 && CheckIfTurn) // right
        {
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationSpeed, 0);
        }
        else if (turnAmount < 0 && CheckIfTurn) // left
        {
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationSpeed, 0);
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
    }

    public override void NoTurning()
    {
        turning = false;
    }

    public override void NoMoving()
    {
        moving = false;
    }

    private void CenterWheel(float CenterSpeed)
    {
        if ((int)transform.localEulerAngles.y <= 360 && (int)transform.localEulerAngles.y >= 360 - STEERINGMAX)
        {
            transform.Rotate(0, CenterSpeed * Time.deltaTime * rotationSpeed, 0);
        }
        else if ((int)transform.localEulerAngles.y >= 0 && (int)transform.localEulerAngles.y <= STEERINGMAX)
        {
            transform.Rotate(0, -CenterSpeed * Time.deltaTime * rotationSpeed, 0);
        }
        
        if ((int)transform.localEulerAngles.y >= 359.5f || (int)transform.localEulerAngles.y <= 0.5f)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        }
    }
}