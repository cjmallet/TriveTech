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
    public List<MovementPart> movementParts;
    private List<Collider> colliders = new List<Collider>();
    private Rigidbody rigidBody;
    private Vector3 movement, rotation;
    public int movementSpeed { get; set; }

    public VehicleMovement()
    {
        
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        movementParts = FindObjectsOfType<MovementPart>().ToList();

        if (movementParts.Count != 0)
        {
            foreach (MovementPart part in movementParts)
            {
                movementSpeed += part.speedModifier;
            }
        }
    }

    private void FixedUpdate()
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        if (enabled && context.performed)
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (!colliders.Contains(collision.GetContact(i).thisCollider))
                    colliders.Add(collision.GetContact(i).thisCollider);
            }
        }
        else if (collision.gameObject.tag != "Ground")
        {

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (colliders.Contains(collision.GetContact(i).thisCollider))
                    colliders.Remove(collision.GetContact(i).thisCollider);
            }
        }
        else if (collision.gameObject.tag != "Ground")
        {

        }
    }
}