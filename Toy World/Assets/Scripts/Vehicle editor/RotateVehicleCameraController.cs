using UnityEngine;

public class RotateVehicleCameraController : MonoBehaviour
{
    // Speed factors for vehicle rotation and camera zooming
    [Range(1,3)]
    public float horizontalRotateSpeed, verticalRotateSpeed, cameraZoomSpeed;

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
        movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }

    private void FixedUpdate()
    {
        // Wordt in toekomst aangeroepn met Unity event en Input Actions
        RotateVehicle(movementInput);
    }

    /* Wordt in de toekomst something like:
     * private void RotateVehicle(InputAction.CallbackContext context)
     * {
     *      Vector3 inputVector = context.ReadValue<Vector3>();
     *      vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
            inputVector.x * -horizontalRotateSpeed, 0, Space.World);
     * }
     */
    private void RotateVehicle(Vector3 inputVector)
    {
        vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
            inputVector.x * -horizontalRotateSpeed, 0, Space.World);
    }
}
