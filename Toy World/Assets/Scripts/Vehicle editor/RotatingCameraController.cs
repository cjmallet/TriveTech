using UnityEngine;
using UnityEngine.InputSystem;

public class RotatingCameraController : MonoBehaviour
{
    public const int ZOOM_SPEED_DAMPNER = 10;

    // Vector used to store the input from the InputActions
    private Vector3 inputVector;

    private float zoomValue;

    // Speed factors for vehicle rotation and camera zooming
    [Range(1,3)]
    public float horizontalRotateSpeed, verticalRotateSpeed, cameraZoomSpeed;

    // Distance between the camera and the core block
    public int minZoomOutDistance, maxZoomOutDistance;

    // Reference to core block so it can be rotated
    public GameObject vehicleCore;

    private void FixedUpdate()
    {
        RotateVehicleMovement(inputVector);
    }

    public virtual void RotateVehicleMovement(Vector3 inputVector)
    {
        inputVector.z = zoomValue;
        inputVector.Normalize();
        Zoom(inputVector.z);
    }

    // Wordt aangeroepen met Unity event en Input Actions
    public void GetInputVector(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void GetZoomValue(InputAction.CallbackContext value)
    {
        zoomValue = value.ReadValue<float>();
    }

    //! Lets the player zoom in and out of the vehicle
    /*!
     * Zoom limit is calculated based (z-axis) distance towards core block.
     * this prevents the player from moving through it (or to far away)
     */
    private void Zoom(float zoomDirection)
    {
        zoomDirection /= ZOOM_SPEED_DAMPNER;

        Vector3 newPosition = transform.position + transform.forward * zoomDirection * cameraZoomSpeed;

        float newDistance = (vehicleCore.transform.position - newPosition).magnitude;

        if ((newDistance > minZoomOutDistance) && (newDistance < maxZoomOutDistance))
        {
            transform.position = newPosition;
        }
    }
}
