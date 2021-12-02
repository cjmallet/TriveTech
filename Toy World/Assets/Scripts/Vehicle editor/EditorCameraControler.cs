using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraControler : RotatingCameraController
{
    /* Wordt in de toekomst something like:
     * public override void RotateVehicle(InputAction.CallbackContext context)
     * {
     *      Vector3 inputVector = context.ReadValue<Vector3>();
     *      transform.RotateAround(vehicleCore.transform.position, Vector3.up, -inputVector.x * horizontalRotateSpeed);
     *      if (transform.rotation.eulerAngles.x + inputVector.y * verticalRotateSpeed < 89 
     *          || transform.rotation.eulerAngles.x + inputVector.y* verticalRotateSpeed > 271)
     *      {
     *          transform.RotateAround(vehicleCore.transform.position, transform.right, inputVector.y * verticalRotateSpeed);
     *      }
     *      base.RotateVehicleMovement(inputVector);
     * }
     */
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        transform.RotateAround(vehicleCore.transform.position, Vector3.up, -inputVector.x * horizontalRotateSpeed);

        if (transform.rotation.eulerAngles.x + inputVector.y * verticalRotateSpeed < 89 
            || transform.rotation.eulerAngles.x + inputVector.y* verticalRotateSpeed > 271)
        {
            transform.RotateAround(vehicleCore.transform.position, transform.right, inputVector.y * verticalRotateSpeed);
        }

        base.RotateVehicleMovement(inputVector);
    }
}
