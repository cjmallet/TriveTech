using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldInputManager : MonoBehaviour
{
    [SerializeField] private VehicleMovement vehicleMovement;
    [HideInInspector] public string keyPressed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal")<0)
        {
            vehicleMovement.MoveLeft();
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            vehicleMovement.MoveRight();
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            vehicleMovement.MoveForward();
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            vehicleMovement.MoveBackward();
        }
        if(Input.GetAxis("Horizontal")==0)
        {
            vehicleMovement.Idle();
        }
    }
}
