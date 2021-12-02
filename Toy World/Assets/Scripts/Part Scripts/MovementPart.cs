using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementPart : Part
{
    public bool grounded { get; set; }

    public int moveSpeedModifier { get; set; }
    public int rotationSpeedModifier { get; set; }
    public float mass { get; set; }

    public MovementPart()
    {
        
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {

    }

    public virtual bool IsGrounded()
    {
        Ray ray = new Ray(new Vector3(transform.position.x,
            transform.position.y - 0.4f, transform.position.z), -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f))
        {
            return true;        
        }
        else
        {
            return false;
        }
    }

    /*
    public virtual void ForwardAction(float moveAmount)
    {
        
    }

    public virtual void BackwardAction(float moveAmount)
    {

    }

    public virtual void LeftAction(float turnAmount)
    {

    }

    public virtual void RightAction(float turnAmount)
    {

    }

    public virtual void StopAction(bool stopped)
    {

    }*/ // Old system
}