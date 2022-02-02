using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pass a target vector and it will home in on this point using rigid body physics.
/// Used by the MissileLauncher class.
/// By Ruben de Graaf
/// </summary>
public class HomingMissile : MonoBehaviour
{

    public Vector3 target;//change to transform or gameobject if you wanna track moving stuff

    //public float rotationSpeed;
    //public float velocity;
    private Rigidbody _rb;

    [SerializeField] private GameObject _explosionFX, _smokeTrail;

    public GameObject launcher;
    public float maxLifespan;
    public float explosionRadius;
    private float _timeAlive;

    [Header("Forward force values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _force;

    [Header("Rotational force values")]
    [SerializeField] private float _rotationForce;//the speed at which it can change direction
    [SerializeField] private float _maxAngularVelocity;
    [SerializeField] private float _angularDrag;
    
    private bool tracking;
    private bool collisionEnabled = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = _maxAngularVelocity;
        _rb.angularDrag = _angularDrag;
        GetComponent<Collider>().enabled = false;
        tracking = true;
    }

    private void FixedUpdate()
    {
        LifeTimer();

        if (tracking)
        {
            HandleMissileSound();

            if (target != Vector3.zero)
            {
                HomeInOnTarget();
            }
            else
            {
                //StopTracking();
                JustGoStraight();
            }
        }

        if (!collisionEnabled)
        {
            LauncherDistanceCheck();
        }
    }

    /// <summary>
    /// Explodes on contact with another collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        FuckingExplode();
    }

    /// <summary>
    /// When a target is given, use the set variables to home in towards the target using rigidBody physics
    /// </summary>
    private void HomeInOnTarget()
    {
        //add velocity. rockets go woosh
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + transform.up * _force, _maxSpeed);

        //Rotate by setting angular velocity
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, target - transform.position);
        float angleInDegrees;
        Vector3 rotationAxis;
        targetRot.ToAngleAxis(out angleInDegrees, out rotationAxis);
        Vector3 angularDisplacement = rotationAxis * angleInDegrees * Mathf.Deg2Rad;

        _rb.angularVelocity = 
            _rb.angularVelocity 
            + (angularDisplacement 
            * angularDisplacement.magnitude
            * _rotationForce
            * 0.1f);
    }

    /// <summary>
    /// When no target is given or target is lost, just fly straight ahead.
    /// </summary>
    private void JustGoStraight()
    {
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + transform.up * _force, _maxSpeed);
    }

    /// <summary>
    /// Alternative, currently unused behaviour, where the missile will stop it's engine, 
    /// lower it's remaining lifetime and fall to the floor when it's target is lost. 
    /// </summary>
    private void StopTracking()
    {
        tracking = false;
        _smokeTrail.GetComponent<ParticleSystem>().Stop();
        //shorten lifespan and drop to the floor
        if ( maxLifespan - _timeAlive >= 1.5f) { }
            _timeAlive = maxLifespan - 1.5f;
        _rb.useGravity = true;
    }

    /// <summary>
    /// Checks if there's a bit of distance between the launcher and the missile before turning the collider on 
    /// to prevent it from colliding with the launcher itself and exploding right away.
    /// </summary>
    private void LauncherDistanceCheck()
    {
        if(!launcher || Vector3.Distance(transform.position, launcher.transform.position) > 1.5f)
        {
            GetComponent<Collider>().enabled = true;
            collisionEnabled = true;
        }
    }

    /// <summary>
    /// Tracks it's current _timeAlive and maxLifespan. Explode if it's lifetime expires.
    /// </summary>
    private void LifeTimer()
    {
        _timeAlive += Time.deltaTime;
        if (_timeAlive >= maxLifespan)
            FuckingExplode();
    }

    /// <summary>
    /// Detaches the _smokeTrail object from the missile and adds a self destruct timer to it. 
    /// This is so that the smoke trail can dissipate normally after the missile explodes and is destroyed.
    /// </summary>
    private void DetachSmokeTrail()
    {
        if(_smokeTrail)
        {
            _smokeTrail.AddComponent<TimedSelfDestruct>().maxLifeSpan = 5f;
            _smokeTrail.transform.parent = null;
            _smokeTrail = null;
        }
    }

    /// <summary>
    /// Explode the missile. This enables the explosion effect, detaches the smoke, 
    /// calls a sound and destroys nearby entities that are affected by explosions.
    /// </summary>
    public void FuckingExplode()
    {
        _explosionFX.AddComponent<TimedSelfDestruct>().maxLifeSpan = 5f;
        _explosionFX.transform.parent = null;
        _explosionFX.gameObject.SetActive(true);
        DetachSmokeTrail();
        HandleMissileExplodingSound();
        foreach (Collider col in Physics.OverlapSphere(transform.position, explosionRadius))
        {
            if (col.gameObject.TryGetComponent(out ShootingEnemy enemy))
            {
                Destroy(enemy.gameObject);
            }
            else if (col.gameObject.TryGetComponent(out MoveObstacle enemyAlso))
            {
                Destroy(enemyAlso.gameObject);
            }
            else if (col.gameObject.TryGetComponent(out DestructibleObject destructibleObject))
            {
                Destroy(destructibleObject.gameObject);
            }
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Plays the rocket engine sound from the audiosource component on this.gameObject.
    /// </summary>
    private void HandleMissileSound()
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            AudioManager.Instance.Play(AudioManager.clips.ButtBoost, gameObject.GetComponent<AudioSource>());
            gameObject.GetComponent<AudioSource>().loop = true;
        }
    }

    /// <summary>
    /// Explosion sound from the AudioManager that plays when, you guessed it, the missile explodes.
    /// </summary>
    private void HandleMissileExplodingSound()
    {
        AudioManager.Instance.Stop(gameObject.GetComponent<AudioSource>());

        GameObject audioSourceMissileExplode = AudioManager.Instance.GetPooledAudioSourceObject();
        audioSourceMissileExplode.transform.localPosition = gameObject.transform.position;
        audioSourceMissileExplode.SetActive(true);

        AudioManager.Instance.Play(AudioManager.clips.MissileExplosion, audioSourceMissileExplode.GetComponent<AudioSource>());
    }

    /// <summary>
    /// Used to visualize missile targets. Uncomment when this becomes relevant for debugging.
    /// </summary>
    private void OnDrawGizmos()
    {
        /*
        visualize tracking target
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target,  0.45f);
        */
    }
}
