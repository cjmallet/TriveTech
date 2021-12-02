using UnityEngine;

public class RotateVehicleCameraController : RotatingCameraController
{
    // Input value for resetting vehicle rotation
    bool resetVehicle = false;

    // Temporary input in Update
    public override void Update()
    {
        base.Update();

        // Random button for resetting rotation
        resetVehicle = Input.GetKey(KeyCode.F);
    }

    public override void FixedUpdate()
    {
        // Wordt in toekomst aangeroepn met Unity event en Input Actions
        base.FixedUpdate();
        ResetVehicleRotation(resetVehicle);
    }

    /* Wordt in de toekomst something like:
     * private void RotateVehicle(InputAction.CallbackContext context)
     * {
     *      Vector3 inputVector = context.ReadValue<Vector3>();
     *      vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
     *      inputVector.x * -horizontalRotateSpeed, 0, Space.World);
     *      Zoom(inputVector.z);
     * }
     */
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
            inputVector.x * -horizontalRotateSpeed, 0, Space.World);
        Zoom(inputVector.z);
        Debug.Log("help");
    }

    //! Lets the player zoom in and out of the vehicle
    /*!
     * Zoom limit is calculated based (z-axis) distance towards core block.
     * this prevents the player from moving through it (or to far away)
     */
    public override void Zoom(float zoomDirection)
    {
        zoomDirection /= ZOOM_SPEED_DAMPNER;

        Vector3 newPosition = transform.position + transform.forward * zoomDirection * cameraZoomSpeed;

        float newDistance = vehicleCore.transform.position.z - newPosition.z;

        if ((newDistance > minZoomOutDistance) && (newDistance < maxZoomOutDistance))
        {
            transform.position = newPosition;
        }
    }

    //! Resets vehicle rotation to original default (0)
    private void ResetVehicleRotation(bool value)
    {
        if (value)
        {
            vehicleCore.transform.rotation = new Quaternion(0, 0, 0, 0);
            Debug.Log("Reset");
        }
    }
    /* Ook hier weer integratie voor new input system:
     * private void ResetVehicleRotation(InputAction.CallbackContext value)
     * {
     *      if (value.started)
     *      {
     *          vehicleCore.transform.rotation = new Quaternion(0, 0, 0, 0);        
     *      }  
     * }
     */
}
