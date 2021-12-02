using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraControler : RotatingCameraController
{
    // Speed factors for vehicle rotation and camera zooming
    [Range(1, 3)]
    public float horizontalRotateSpeed, verticalRotateSpeed, cameraZoomSpeed;

    
}
