using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPart : UtilityPart
{
    const int FORCE_MULTIPLIER = 1000;

    private bool jumpIsReady = true;

    [SerializeField] [Range(0,3)]
    private float jumpStrenght;

    public bool PlayAnimation { get; set; }

    private bool restart = false;

    private void FixedUpdate()
    {
        // Applies boost each frame while boost timer is still running,
        // otherwise stops the boost.
        if (DoAction)
        {
            Jump();
            PlayAnimation = true;
            DoAction = false;
            if (restart)
                restart = false;
        }

        if (PlayAnimation)
            JumpAnimation();
    }

    /// <summary>
    /// Resets boost duration timer and turns on DoAction to set boost in motion.
    /// This is only done if the boost is ready based on the recharge timer.
    /// </summary>
    public override void UtilityAction()
    {
        if (!DoAction && jumpIsReady)
        {
            DoAction = true;
            jumpIsReady = false;
        }
        else jumpIsReady = true;
    } 

    /// <summary>
    /// Applies jump with imulse force.
    /// </summary>
    private void Jump()
    {
        int layermask = 1 << 3;
        layermask = ~layermask;
        RaycastHit surfaceHit;
        if (Physics.Raycast(transform.position, -transform.up, out surfaceHit, 0.7f, layermask))
        {
            transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(
            FORCE_MULTIPLIER * jumpStrenght * transform.up, transform.position, ForceMode.Impulse);
        }
    }

    public virtual void JumpAnimation()
    {

    }

    /// <summary>
    /// Reset the booster values like it's used for the first time
    /// </summary>
    public override void ResetAction()
    {
        restart = true;
    }
}
