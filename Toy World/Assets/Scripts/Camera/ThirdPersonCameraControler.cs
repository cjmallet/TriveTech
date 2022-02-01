using UnityEngine;

/// <summary>
/// Rotates the playmode camera around the vehicle using a camera pivot point.
/// </summary>
public class ThirdPersonCameraControler : RotatingCameraController
{
    private const int UP_ROTATION_MAX = 85;
    private const int DOWN_ROTATION_MAX = 275;
    private const int ROTATION_SPEED_DAMPNER = 5;

    private GameObject vehicleCore;
    private GameObject cameraPivot;

    /// <summary>
    /// Dampen the rotation speeds and initialises the vehicle core and camera pivot.
    /// </summary>
    private void Awake()
    {
        verticalRotateSpeed /= ROTATION_SPEED_DAMPNER;
        horizontalRotateSpeed /= ROTATION_SPEED_DAMPNER;

        vehicleCore = GameManager.Instance.vehicleEditor.coreBlockPlayMode;
        cameraPivot = gameObject.transform.parent.gameObject;

        FollowVehicle();
    }

    /// <summary>
    /// Rotates the camera around the vehicle (camera pivot) based on an input vector. 
    /// </summary>
    /// <param name="inputVector">Vector received from input system.</param>
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        FollowVehicle();

        base.RotateVehicleMovement(inputVector);

        transform.RotateAround(cameraPivot.transform.position, Vector3.up, inputVector.x * horizontalRotateSpeed);
        
        if (transform.rotation.eulerAngles.x + -inputVector.y * verticalRotateSpeed < UP_ROTATION_MAX
            || transform.rotation.eulerAngles.x + -inputVector.y * verticalRotateSpeed > DOWN_ROTATION_MAX)
        {
            transform.RotateAround(cameraPivot.transform.position, transform.right, -inputVector.y * verticalRotateSpeed);
        }
    }

    /// <summary>
    /// Simply sets camera pivot at same location as the vehicle core.
    /// </summary>
    private void FollowVehicle()
    {
        cameraPivot.transform.position = vehicleCore.transform.position;
    }
}
