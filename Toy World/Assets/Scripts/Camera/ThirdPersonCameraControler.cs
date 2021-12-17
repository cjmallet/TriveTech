using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControler : RotatingCameraController
{
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        base.RotateVehicleMovement(inputVector);

        transform.RotateAround(vehicleCore.transform.position, Vector3.up, inputVector.x * (horizontalRotateSpeed/10));

        if (transform.rotation.eulerAngles.x + -inputVector.y * verticalRotateSpeed < 89 
            || transform.rotation.eulerAngles.x + -inputVector.y* verticalRotateSpeed > 271)
        {
            transform.RotateAround(vehicleCore.transform.position, transform.right, -inputVector.y * (verticalRotateSpeed / 10));
        }
    }
}
