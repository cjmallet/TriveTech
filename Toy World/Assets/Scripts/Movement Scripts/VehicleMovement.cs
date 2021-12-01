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
            foreach (ContactPoint contact in collision.contacts.Where(x => !colliders.Contains(x.thisCollider) && 
            x.thisCollider.gameObject.TryGetComponent(out MovementPart part))) // gotta fix memory issue
            {
                colliders.Add(contact.thisCollider);
                contact.thisCollider.gameObject.GetComponent<MovementPart>().grounded = true;
            }
        }
        else if (collision.gameObject.tag != "Ground")
        {
            foreach (ContactPoint contact in collision.contacts.Where(x => !colliders.Contains(x.thisCollider)))
            {
                // do something
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            foreach (ContactPoint contact in collision.contacts.Where(x => colliders.Contains(x.thisCollider) && 
            x.thisCollider.gameObject.TryGetComponent(out MovementPart part)))
            {
                colliders.Remove(contact.thisCollider);
                contact.thisCollider.gameObject.GetComponent<MovementPart>().grounded = false;
            }
        }
        else if (collision.gameObject.tag != "Ground")
        {

        }
    }
}