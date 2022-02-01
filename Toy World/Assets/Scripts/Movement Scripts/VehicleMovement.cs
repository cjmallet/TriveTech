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
    [HideInInspector]
    public List<Part> allParts = new List<Part>();
    public List<WheelInfo> wheelInfos = new List<WheelInfo>();
    private float motor, steering;
    private Rigidbody rigidBody;
    private Vector3 moveVector;

    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>(); // Get the rigidbody so we can actually get it moving.

        if (allParts.Count != 0) // If we have our collection of vehicle parts, add them to the rigidbody weight.
        {
            foreach (Part part in allParts)
            {
                rigidBody.mass += part.weight;
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            if (wheelInfo.motor) // If the wheel is a motor part
            {
                motor = wheelInfo.maxMotorTorgue * moveVector.z; // Apply it's motor based rotation.
                wheelInfo.wheel.motorTorque = motor;
            }

            if (wheelInfo.steering) // If the wheel is a steering part
            {
                steering = wheelInfo.maxSteering * moveVector.x; // Apply steering angle if steering.
                wheelInfo.wheel.steerAngle = steering;
            }
        }
        Debug.Log(rigidBody.velocity.magnitude);
    }

    /// <summary>
    /// Collects input from player.
    /// </summary>
    /// <param name="context">Input variable.</param>
    public void Move(InputAction.CallbackContext context)
    {
        AudioManager.Instance.vehicleMove = moveVector = context.ReadValue<Vector3>();
    }

    /// <summary>
    /// Adds a wheel to the collection of wheels in the vehicle movement.
    /// </summary>
    /// <param name="part">Part to add.</param>
    public void AddWheel(MovementPart part)
    {
        WheelInfo wheel = new WheelInfo(); // Make a new 'wheel'
        wheel.wheel = part.gameObject.GetComponent<WheelCollider>();

        if (part.frontPart) // Steering wheel
            wheel.steering = true;
        else // Engine wheel
            wheel.steering = false;

        wheel.maxMotorTorgue = part.maxTorgue;
        wheel.maxSteering = part.steeringAngle;
        wheel.motor = true;                 // Apply all applicapble information to the wheel.

        wheelInfos.Add(wheel);
    }

    [System.Serializable]
    public class WheelInfo
    {
        public WheelCollider wheel;
        public bool motor; // Attached to motor?
        public bool steering; // Attached to steer angle?
        public float maxMotorTorgue;
        public float maxSteering;
    }
}