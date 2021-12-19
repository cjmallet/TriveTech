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
    public float maxMotorTorgue, maxSteeringAngle;
    private float motor, steering;
    private Rigidbody rigidBody;
    private Vector3 moveVector;

    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (allParts.Count != 0)
        {
            foreach (Part part in allParts)
            {
                rigidBody.mass += part.weight;
            }
        }
    }

    private void FixedUpdate()
    {
        motor = maxMotorTorgue * moveVector.z;
        steering = maxSteeringAngle * moveVector.x;

        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            if (wheelInfo.motor)
            {
                wheelInfo.wheel.motorTorque = motor;
            }

            if (wheelInfo.steering)
            {
                wheelInfo.wheel.steerAngle = steering;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector3>();
    }

    public void AddWheel(MovementPart part)
    {
        WheelInfo wheel = new WheelInfo();
        wheel.wheel = part.gameObject.GetComponent<WheelCollider>();

        if (part.frontPart) // Steering wheel
        {
            wheel.motor = false;
            wheel.steering = true;
        }
        else // Engine wheel
        {
            wheel.motor = true;
            wheel.steering = false;
        }

        wheelInfos.Add(wheel);
    }

    [System.Serializable]
    public class WheelInfo
    {
        public WheelCollider wheel;
        public bool motor; // Attached to motor?
        public bool steering; // Attached to steer angle?
    }
}