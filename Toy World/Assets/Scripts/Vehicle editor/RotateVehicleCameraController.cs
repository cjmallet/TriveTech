using UnityEngine;
using UnityEngine.InputSystem;

public class RotateVehicleCameraController : RotatingCameraController
{
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        vehicleCore.transform.Rotate(inputVector.z * verticalRotateSpeed, 
            inputVector.x * -horizontalRotateSpeed, 0, Space.World);
        base.RotateVehicleMovement(inputVector);
    }

    //! Resets vehicle rotation to original default (0)
    public void ResetVehicleRotation(InputAction.CallbackContext value)
    {
         if (value.started)
         {
             vehicleCore.transform.rotation = new Quaternion(0, 0, 0, 0);        
         }  
    }   
}
