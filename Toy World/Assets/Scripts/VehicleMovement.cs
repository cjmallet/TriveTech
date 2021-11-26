using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move the vehicle based on the input from the inputManager class
/// </summary>
public class VehicleMovement : MonoBehaviour
{
    public List<MovementPart> movementParts;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float steeringAngle;

    public VehicleMovement()
    {
        movementSpeed = 0.1f;
        steeringAngle = 1;
    }

    //! Input check in Update
    private void Update()
    {
        if (movementParts.Count != 0)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                MoveLeft();
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                MoveRight();
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                MoveForward();
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                MoveBackward();
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                Idle();
            }
        }
    }

    public void MoveLeft()
    {
        transform.Rotate(0, -steeringAngle, 0);
        // each movement part activates left movement action
        foreach (MovementPart movementPart in movementParts)
        {
            movementPart.LeftAction(steeringAngle);
        }
    }

    public void MoveRight()
    {
        transform.Rotate(0, steeringAngle, 0);
        foreach (MovementPart movementPart in movementParts)
        {
            movementPart.RightAction(steeringAngle);
        }
    }

    public void MoveForward()
    {
        transform.localPosition += transform.forward * movementSpeed;
        foreach (MovementPart movementPart in movementParts)
        {
            movementPart.ForwardAction(movementSpeed);
        }
    }

    public void MoveBackward()
    {
        transform.localPosition += -transform.forward * movementSpeed;
        foreach (MovementPart movementPart in movementParts)
        {
            movementPart.BackwardAction(movementSpeed);
        }
    }

    public void Idle()
    {
        foreach (MovementPart movementPart in movementParts)
        {
            movementPart.StopAction(false);
        }
    }
}
