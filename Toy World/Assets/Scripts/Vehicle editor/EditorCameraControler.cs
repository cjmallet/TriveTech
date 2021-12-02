using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraControler : RotatingCameraController
{
    /* Wordt in de toekomst something like:
     * public override void RotateVehicle(InputAction.CallbackContext context)
     * {
     *      Vector3 inputVector = context.ReadValue<Vector3>();
     *      vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed, 
     *      inputVector.x * -horizontalRotateSpeed, 0, Space.World);
     *      Zoom(inputVector.z);
     * }
     */
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        //vehicleCore.transform.Rotate(inputVector.y * verticalRotateSpeed,
        //    inputVector.x * -horizontalRotateSpeed, 0, Space.World);

        transform.RotateAround(vehicleCore.transform.position, transform.up, -inputVector.x * horizontalRotateSpeed);

        base.RotateVehicleMovement(inputVector);
    }
}
