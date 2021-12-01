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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Temporary input in Update
    void Update()
    {
        float movementInputZ = 0;
        if (Input.GetKey(KeyCode.Space))
            movementInputZ = 1;
        if (Input.GetKey(KeyCode.LeftShift))
            movementInputZ = -1;

        movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), movementInputZ);
    }

    private void FixedUpdate()
    {
        // Wordt in toekomst aangeroepn met Unity event en Input Actions
        RotateVehicleMovement(movementInput);
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

        float distance = vehicleCore.transform.position.z - transform.position.z;
        if (distance < minZoomOutDistance)
        {
            transform.Translate(0, 0, (distance - minZoomOutDistance));
        }
        else if (distance > maxZoomOutDistance)
        {
            transform.Translate(0, 0, (distance - maxZoomOutDistance));
        }
        else
        {
            transform.position += transform.forward * zoomDirection * cameraZoomSpeed;
        }
    }
}
