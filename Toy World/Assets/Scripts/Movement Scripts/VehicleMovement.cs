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
    private Rigidbody rigidBody;
    private Vector3 eulerRot, movement;
    public int movementSpeed;
    public int rotationSpeed;

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
            foreach (MovementPart part in movementParts)
            {
                rigidBody.mass += part.mass;
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
            if ( part.IsGrounded() && !part.grounded)
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
            movement *= movementSpeed;

            eulerRot = new Vector3(0, context.ReadValue<Vector3>().x, 0);
            eulerRot *= rotationSpeed;
        }
    }
}