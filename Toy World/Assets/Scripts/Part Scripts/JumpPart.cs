using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPart : UtilityPart
{
    const int FORCE_MULTIPLIER = 1500;

    [SerializeField]
    private int rechargeDurationSeconds;
    private float rechargeTimer = 0;
    private bool jumpIsReady = true;

    [SerializeField] [Range(0,3)]
    private float jumpStrenght;

    private bool restart = false;

    private void FixedUpdate()
    {
        // Timer to check if booster can be used again
        if (rechargeTimer > 0)
        {
            rechargeTimer -= Time.deltaTime;
            jumpIsReady = false;
        }
        else if (!jumpIsReady)
        {
            jumpIsReady = true;
            if(!restart)
                StartCoroutine(BoostReadyIndication());
        }

        // Applies boost each frame while boost timer is still running,
        // otherwise stops the boost.
        if (DoAction)
        {
            Jump();
            DoAction = false;
            if (restart)
                restart = false;
        }
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
        }
    } 

    /// <summary>
    /// Applies jump with imulse force.
    /// </summary>
    private void Jump()
    {
        transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(
            FORCE_MULTIPLIER * jumpStrenght * transform.up, transform.position, ForceMode.Impulse);
        rechargeTimer = rechargeDurationSeconds;
    }

    /// <summary>
    /// Reset the booster values like it's used for the first time
    /// </summary>
    public override void ResetAction()
    {
        rechargeTimer = 0;
        restart = true;
    }

    /// <summary>
    /// Short particle effect indication to show boost is recharged
    /// </summary>
    /// <returns></returns>
    private IEnumerator BoostReadyIndication()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
