using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoostPart : UtilityPart
{
    [SerializeField]
    private int boostDurationSeconds;

    [SerializeField]
    private ParticleSystem boostParticles;

    private float boostTimer;

    private void FixedUpdate()
    {
        if (DoAction)
        {
            Boost();
            boostTimer -= Time.deltaTime;
        }
        if (boostTimer <= 0)
        {
            DoAction = false;
            StopBoost();
        }
    }

    public override void UtilityAction()
    {
        if (!DoAction)
        {
            DoAction = true;
            boostTimer = boostDurationSeconds;
        }
    } 

    private void Boost()
    {
        if (!boostParticles.isPlaying)
            boostParticles.Play();
    }

    private void StopBoost()
    {
        boostParticles.Stop();
    }
}
