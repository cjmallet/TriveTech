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
        if (moveAmount != 0)
        {
            moving = true;
            StopCoroutine(MoveWheel(moveAmount));
            StartCoroutine(MoveWheel(moveAmount));
        }
    }

    private IEnumerator MoveWheel(float moveAmount)
    {
        transform.GetChild(1).Rotate(0, -moveAmount * Time.deltaTime * rotationSpeed, 0);
        CenterWheel(CENTERSPEED);

        yield return new WaitForEndOfFrame();

        if (moving)
            yield return StartCoroutine(MoveWheel(moveAmount));
        else
            yield break;
    }

    public override void HorizontalMovement(float turnAmount)
    {
        // left is -1, right is 1
        if (turnAmount != 0)
        {
            turning = true;
            StopCoroutine(TurnWheel(turnAmount));
            StartCoroutine(TurnWheel(turnAmount));
        }          
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
        else
            yield break;
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
        if (transform.localEulerAngles.y != 0 && !turning)
        {
            
        }
    }
}