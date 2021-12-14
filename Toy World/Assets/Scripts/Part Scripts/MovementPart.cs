using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementPart : Part
{
    public bool grounded { get; set; }

    public int moveSpeed;
    public int rotationSpeed;

    public virtual bool IsGrounded()
    {
        Ray ray = new Ray(new Vector3(transform.position.x,
            transform.position.y - 0.4f, transform.position.z), -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            return true;        
        }
        else
        {
            return false;
        }
    }

    public virtual void VerticalMovement(float moveAmount)
    {
        
    }

    public virtual void HorizontalMovement(float turnAmount)
    {

    }

    public virtual void NoTurning()
    {

    }

    public virtual void NoMoving()
    {

    }
}