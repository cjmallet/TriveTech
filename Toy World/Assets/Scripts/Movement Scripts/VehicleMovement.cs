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
    public List<GameObject> colliders = new List<GameObject>();
    private Rigidbody rigidBody;
    private Vector3 eulerRot, movement;
    //private bool collisionUpdate = false;
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
            foreach (MovementPart part in movementParts)
            {
                movementSpeed += part.moveSpeedModifier;
                rotationSpeed += part.rotationSpeedModifier;
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

    /*private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && collisionUpdate)
        {
            for (int i = colliders.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < collision.contactCount; j++)
                {
                    if (colliders.Contains(collision.GetContact(j).thisCollider.gameObject) && collision.GetContact(j).thisCollider.gameObject.TryGetComponent(out MovementPart part))
                    {
                        colliders.Remove(collision.GetContact(j).thisCollider.gameObject);
                        Debug.Log(" REMOVE " + collision.GetContact(j).thisCollider);
                    }
                }
            }

            collisionUpdate = false;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (!colliders.Contains(collision.GetContact(i).thisCollider.gameObject) && collision.GetContact(i).thisCollider.gameObject.TryGetComponent(out MovementPart part))
                    colliders.Add(collision.GetContact(i).thisCollider.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (!colliders.Contains(collision.GetContact(i).thisCollider.gameObject) && collision.GetContact(i).thisCollider.gameObject.TryGetComponent(out MovementPart part))
                {
                    colliders.Add(collision.GetContact(i).thisCollider.gameObject);
                }
            }
        }
        else if (collision.gameObject.tag != "Ground")
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionUpdate = true;
    }*/ // Old collision attempt
}