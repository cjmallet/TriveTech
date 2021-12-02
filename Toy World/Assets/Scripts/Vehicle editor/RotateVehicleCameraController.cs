using UnityEngine;

public class RotateVehicleCameraController : MonoBehaviour
{
    const int ZOOM_SPEED_DAMPNER = 10;

    // Speed factors for vehicle rotation and camera zooming
    [Range(1,3)]
    public float horizontalRotateSpeed, verticalRotateSpeed, cameraZoomSpeed;

    // Distance between the camera and the core block
    public int minZoomOutDistance, maxZoomOutDistance;

    // Reference to core block so it can be rotated
    public GameObject vehicleCore;

    // Movement vector formed from input
    private Vector3 movementInput;

    // Input value for resetting vehicle rotation
    bool resetVehicle = false;

    // Temporary input in Update
    void Update()
    {
        float movementInputZ = 0;
        if (Input.GetKey(KeyCode.Space))
            movementInputZ = 1;
        if (Input.GetKey(KeyCode.LeftShift))
            movementInputZ = -1;

        movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), movementInputZ);

        // Random button for resetting rotation
        resetVehicle = Input.GetKey(KeyCode.F);
    }

    private void FixedUpdate()
    {
        // Wordt in toekomst aangeroepn met Unity event en Input Actions
        RotateVehicleMovement(movementInput);
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
    private void RotateVehicleMovement(Vector3 inputVector)
    {
        vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
            inputVector.x * -horizontalRotateSpeed, 0, Space.World);
        Zoom(inputVector.z);
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
