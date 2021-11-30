using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPart : Part
{
    public MovementPart()
    {
        
    }

    private void Awake()
    {
        ShowFrontDirection();
    }

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

    }
}
