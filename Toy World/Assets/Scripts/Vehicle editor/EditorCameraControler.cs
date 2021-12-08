using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraControler : RotatingCameraController
{
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        transform.RotateAround(vehicleCore.transform.position, Vector3.up, -inputVector.x * horizontalRotateSpeed);

        if (transform.rotation.eulerAngles.x + inputVector.z * verticalRotateSpeed < 89 
            || transform.rotation.eulerAngles.x + inputVector.z* verticalRotateSpeed > 271)
        {
            transform.RotateAround(vehicleCore.transform.position, transform.right, inputVector.z * verticalRotateSpeed);
        }

        base.RotateVehicleMovement(inputVector);
    }
}
