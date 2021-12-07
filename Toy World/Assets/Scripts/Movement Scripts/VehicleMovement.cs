using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Move the vehicle based on the input from the inputManager class
/// </summary>
public class VehicleMovement : MonoBehaviour
{
    public List<MovementPart> movementParts = new List<MovementPart>();
    [HideInInspector]
    public List<Part> allParts = new List<Part>();
    private Rigidbody rigidBody;
    private Vector3 eulerRot, movement;
    public int movementSpeed { get; set; }
    public int rotationSpeed { get; set; }

    public VehicleMovement()
    {
        
    }

    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();

        movementSpeed = 0;
        rotationSpeed = 0;

        if (movementParts.Count != 0)
        {
            foreach (Part part in allParts)
            {
                rigidBody.mass += part.Weight;
            }
        }
    }

    private void OnDisable()
    {
        movementSpeed = 0;
        rotationSpeed = 0;

        if (movementParts.Count != 0)
        {
            foreach (MovementPart part in movementParts)
            {
                part.grounded = false;
            }
        }
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        rigidBody.AddRelativeForce(movement);

        Quaternion deltaRot = Quaternion.Euler(eulerRot * Time.fixedDeltaTime);
        rigidBody.MoveRotation(rigidBody.rotation * deltaRot);

        foreach (MovementPart part in movementParts)
        {
            if (part.IsGrounded() && !part.grounded)
            {
                movementSpeed += part.moveSpeedModifier;
                rotationSpeed += part.rotationSpeedModifier;
                part.grounded = true;
            }
            else if (!part.IsGrounded() && part.grounded)
            {
                movementSpeed -= part.moveSpeedModifier;
                rotationSpeed -= part.rotationSpeedModifier;
                part.grounded = false;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (enabled && context.performed)
        {
            movement = new Vector3(0, 0, context.ReadValue<Vector3>().z);
            eulerRot = new Vector3(0, context.ReadValue<Vector3>().x, 0);

            movement *= movementSpeed;
            eulerRot *= rotationSpeed;

            if (eulerRot.y == 0)
            {
                foreach (MovementPart part in movementParts)
                    part.NoTurning();
            }
            
            if (movement.z == 0)
            {
                foreach (MovementPart part in movementParts)
                    part.NoMoving();
            }
            
            if (eulerRot.y != 0 || movement.z != 0)
            {
                foreach (MovementPart part in movementParts)
                {
                    part.VerticalMovement(moveAmount: movement.z / movementParts.Count);
                    part.HorizontalMovement(turnAmount: eulerRot.y / movementParts.Count);
                }
            }
        }
    }
}