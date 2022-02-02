using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is Unity's camera controller edited to fit our purposes.
/// </summary>
public class FPSCameraControllers : MonoBehaviour
{
    public class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform transform)
        {
            pitch = transform.eulerAngles.x;
            yaw = transform.eulerAngles.y;
            roll = transform.eulerAngles.z;
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
        }

        public void Translate(Vector3 translation, Transform transform)
        {
            Vector3 betterTranslation = Vector3.zero;
            betterTranslation += translation.x * transform.right;
            betterTranslation += translation.y * Vector3.up;
            betterTranslation += translation.z * transform.forward;
            x += betterTranslation.x;
            y += betterTranslation.y;
            z += betterTranslation.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform transform)
        {
            transform.eulerAngles = new Vector3(pitch, yaw, roll);
            transform.position = new Vector3(x, y, z);
        }
    }

    private Vector2 lookRotation = Vector2.zero;
    private Vector3 direction = Vector3.zero;

    public static bool canRotate = false;
    const float k_MouseSensitivityMultiplier = 0.01f;

    [HideInInspector] public CameraState m_TargetCameraState = new CameraState();
    [HideInInspector] public CameraState m_InterpolatingCameraState = new CameraState();

    [Header("Movement Settings")]
    [Tooltip("Speed of the player character"), Range(3f, 20f)]
    public float movementSpeed = 10f;
    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 0.2f)]
    public float mouseSensitivity = 0.1f;

    [Header("Rotation Settings")]
    [Tooltip("Multiplier for the sensitivity of the rotation.")]
    public float rotationMultiplier = 60.0f;

    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;


    void OnEnable()
    {
        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate && (lookRotation != Vector2.zero || direction != Vector3.zero) && enabled)
            CameraRotation(lookRotation, direction);
    }


    public void CameraRotation(Vector2 lookRotation, Vector3 direction)
    {
        var mouseMovement = lookRotation * k_MouseSensitivityMultiplier * rotationMultiplier;
        if (invertY)
            mouseMovement.y = -mouseMovement.y;

        var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

        m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
        m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;


        var translation = direction * Time.deltaTime * movementSpeed;

        m_TargetCameraState.Translate(translation, gameObject.transform);

        //Debug.Log("main camera position: " + gameObject.transform.position + "          main camera rotation: " + gameObject.transform.rotation);

        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / mouseSensitivity) * Time.deltaTime);
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);

        m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);
        m_InterpolatingCameraState.UpdateTransform(transform);
    }

    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector3>();
    }

    public void MouseMove(InputAction.CallbackContext context)
    {
        lookRotation = context.ReadValue<Vector2>() * mouseSensitivity;
    }
}
