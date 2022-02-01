using UnityEngine;

public class ThirdPersonCameraControler : RotatingCameraController
{
    private const int UP_ROTATION_MAX = 85;
    private const int DOWN_ROTATION_MAX = 275;
    private const int ROTATION_SPEED_DAMPNER = 5;

    private GameObject vehicleCore;

    private void Awake()
    {
        verticalRotateSpeed /= ROTATION_SPEED_DAMPNER;
        horizontalRotateSpeed /= ROTATION_SPEED_DAMPNER;

        vehicleCore = GameManager.Instance.vehicleEditor.coreBlockPlayMode;
    }

    /// <summary>
    /// Rotates the camera around the vehicle based on an input vector.
    /// Always locks the z rotation and locks the x rotation if the  
    /// </summary>
    /// <param name="inputVector">Vector received from input system.</param>
    public override void RotateVehicleMovement(Vector3 inputVector)
    {
        base.RotateVehicleMovement(inputVector);
        transform.RotateAround(vehicleCore.transform.position, Vector3.up, inputVector.x * horizontalRotateSpeed);

        if (transform.rotation.eulerAngles.x + -inputVector.y * verticalRotateSpeed < UP_ROTATION_MAX
            || transform.rotation.eulerAngles.x + -inputVector.y * verticalRotateSpeed > DOWN_ROTATION_MAX)
        {
            transform.RotateAround(vehicleCore.transform.position, transform.right, -inputVector.y * verticalRotateSpeed);
        }

        // Lock z rotation
        Vector3 currentEulerAngles = transform.eulerAngles;
        currentEulerAngles.z = 0;
        transform.eulerAngles = currentEulerAngles;

        FollowVehicle();
    }

    private void FollowVehicle()
    {
        Vector3 previousPosition = transform.position;
        Vector3 distance = previousPosition - vehicleCore.transform.position;
        transform.position = previousPosition + distance;
    }
}
