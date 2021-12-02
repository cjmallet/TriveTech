using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPart : Part
{
    public bool grounded { get; set; }

    public int moveSpeedModifier { get; set; }
    public int rotationSpeedModifier { get; set; }
    public float mass { get; set; }

    public MovementPart()
    {
        this.grounded = false;
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