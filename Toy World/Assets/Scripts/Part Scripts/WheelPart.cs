using System.Collections;
using UnityEngine;

public class WheelPart : MovementPart
{
    private float vertSpeed, horSpeed;

    public float steeringAngle { get; set; }
    private const int STEERINGMAX = 30;
    private bool turning = false, moving = false;

    private bool CheckIfTurn { get { return (transform.localEulerAngles.y >= 0 && transform.localEulerAngles.y < STEERINGMAX - 1)
            || (transform.localEulerAngles.y > 360 - STEERINGMAX && transform.localEulerAngles.y <= 360); } }


    public override bool IsGrounded()
    {
        return base.IsGrounded();
    }

    public override void SwitchColliders()
    {
        GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
        GetComponent<WheelCollider>().enabled = !GetComponent<WheelCollider>().enabled;
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
        if (turning && CheckIfTurn)
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationSpeed, 0);
        else if (turning && !CheckIfTurn && turnAmount > 0 && transform.localEulerAngles.y > 300) // right
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationSpeed, 0);
        else if (turning && !CheckIfTurn && turnAmount < 0 && transform.localEulerAngles.y < 60) // left
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationSpeed, 0);
        else if ((int)transform.localEulerAngles.y == 0 && turning)
            transform.Rotate(0, turnAmount * Time.deltaTime * rotationSpeed, 0);
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
        if ((int)transform.localEulerAngles.y <= 360 && (int)transform.localEulerAngles.y >= 300)
        {
            transform.Rotate(0, CenterSpeed * Time.deltaTime * rotationSpeed, 0);
        }
        else if ((int)transform.localEulerAngles.y >= 0 && (int)transform.localEulerAngles.y <= 60)
        {
            transform.Rotate(0, -CenterSpeed * Time.deltaTime * rotationSpeed, 0);
        }
        
        if ((int)transform.localEulerAngles.y >= Mathf.FloorToInt(360f - (rotationSpeed / 10f)) || (int)transform.localEulerAngles.y <= Mathf.CeilToInt(0 + (rotationSpeed / 10f)))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        }
    }
}