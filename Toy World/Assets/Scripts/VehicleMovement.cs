using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move the vehicle based on the input from the inputManager class
/// </summary>
public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private int movementSpeed;
    [HideInInspector] public int steeringAngle;
    [HideInInspector] public bool moveLeft, moveRight;

    public void MoveLeft()
    {
        transform.Rotate(0,steeringAngle, 0);
        moveLeft = true;
    }

    public void MoveRight()
    {
        transform.Rotate(0, -steeringAngle, 0);
        moveRight = true;
    }

    public void MoveForward()
    {
        transform.localPosition += transform.forward * movementSpeed/10f;
    }

    public void MoveBackward()
    {
        transform.localPosition += -transform.forward * movementSpeed/10f;
    }

    public void Idle()
    {
        moveRight= moveLeft = false;
    }
}
