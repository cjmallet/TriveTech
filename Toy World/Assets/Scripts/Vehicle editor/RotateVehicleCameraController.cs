using UnityEngine;
using UnityEngine.InputSystem;

public class RotateVehicleCameraController : RotatingCameraController
{
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        base.RotateVehicleMovement(inputVector);
        vehicleCore.transform.Rotate(inputVector.y * (verticalRotateSpeed / 10), 
            inputVector.x * (-horizontalRotateSpeed / 10), 0, Space.World);
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
