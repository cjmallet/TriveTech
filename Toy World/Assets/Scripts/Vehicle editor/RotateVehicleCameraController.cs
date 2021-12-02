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
     * public override void RotateVehicle(InputAction.CallbackContext context)
     * {
     *      Vector3 inputVector = context.ReadValue<Vector3>();
     *      vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
     *          inputVector.x * -horizontalRotateSpeed, 0, Space.World);
     *      base.RotateVehicleMovement(inputVector);
     * }
     */
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
            inputVector.x * -horizontalRotateSpeed, 0, Space.World);
        base.RotateVehicleMovement(inputVector);
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
