using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gives a boost to the vehicle when this part is activated
/// </summary>
public class BoostPart : UtilityPart
{
    const int FORCE_MULTIPLIER = 800;

    [SerializeField]
    private int rechargeDurationSeconds;
    private float rechargeTimer = 0;
    private bool boostIsReady = true;

    [SerializeField]
    private int boostDurationSeconds;
    private float boostTimer;

    [SerializeField]
    private ParticleSystem boostParticles;

    [SerializeField] [Range(0,3)]
    public float boostStrenght;

    private bool restart = false;

    public AudioManager.clips boostReadyClip, boostSoundClip;

    private void FixedUpdate()
    {
        // Timer to check if booster can be used again
        if (rechargeTimer > 0)
        {
            rechargeTimer -= Time.deltaTime;
            boostIsReady = false;
        }
        else if (!boostIsReady)
        {
            boostIsReady = true;
            if (!restart)
            {
                StartCoroutine(BoostReadyIndication());
                AudioManager.Instance.Play(boostReadyClip, GetComponent<AudioSource>());
            }
                
        }

        // Applies boost each frame while boost timer is still running,
        // otherwise stops the boost.
        if (DoAction)
        {
            Boost();
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                DoAction = false;
                StopBoost();
            }
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
        if (!DoAction && boostIsReady)
        {
            DoAction = true;
            boostTimer = boostDurationSeconds;
        }
    } 

    /// <summary>
    /// Applies boost with force and a particle effect.
    /// </summary>
    private void Boost()
    {
        if (!boostParticles.isPlaying)
        {
            boostParticles.Play();
            GetComponent<AudioSource>().loop = true;
            AudioManager.Instance.Play(boostSoundClip, GetComponent<AudioSource>());
        }

        transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(
            -FORCE_MULTIPLIER * boostStrenght * transform.forward, transform.position, ForceMode.Force);
    }

    /// <summary>
    /// Stops boost by resetting boost duration timer and recharge timer
    /// and stopping the particle effect. 
    /// The boost force is stopped automaticly when the 'Boost' function isn't called.
    /// </summary>
    private void StopBoost()
    {
        boostParticles.Stop();
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Stop();
        boostTimer = boostDurationSeconds;
        rechargeTimer = rechargeDurationSeconds;
    }

    /// <summary>
    /// Reset the booster values like it's used for the first time
    /// </summary>
    public override void ResetAction()
    {
        boostParticles.Stop();
        boostTimer = boostDurationSeconds;
        rechargeTimer = 0;
        restart = true;
    }

    /// <summary>
    /// Short particle effect indication to show boost is recharged
    /// </summary>
    /// <returns></returns>
    private IEnumerator BoostReadyIndication()
    {
        boostParticles.Play();
        yield return new WaitForSeconds(0.2f);
        boostParticles.Stop();
    }
}
