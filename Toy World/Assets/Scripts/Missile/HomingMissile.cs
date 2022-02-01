using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Homing missile by Ruben de Graaf
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

    [Header("Perlin randomness")]
    
    private Vector3 targetPos;
    private bool tracking;
    private bool collisionEnabled = false;


    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = _maxAngularVelocity;
        _rb.angularDrag = _angularDrag;
        GetComponent<Collider>().enabled = false;
        tracking = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        LifeTimer();

        if (tracking)
        {
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

    private void OnCollisionEnter(Collision collision)
    {
        FuckingExplode();
    }

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
    private void JustGoStraight()
    {
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + transform.up * _force, _maxSpeed);
    }

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
    /// Checks if there's a bit of distance between the launcher and the missile before turning the collider on to prevent it from colliding with the launcher itself and exploding right away.
    /// </summary>
    private void LauncherDistanceCheck()
    {
        if(!launcher || Vector3.Distance(transform.position, launcher.transform.position) > 1.5f)
        {
            GetComponent<Collider>().enabled = true;
            collisionEnabled = true;
        }
    }

    private void LifeTimer()
    {
        _timeAlive += Time.deltaTime;
        if (_timeAlive >= maxLifespan)
            FuckingExplode();
    }

    private void DetachSmokeTrail()
    {
        if(_smokeTrail)
        {
            _smokeTrail.AddComponent<TimedSelfDestruct>().maxLifeSpan = 5f;
            _smokeTrail.transform.parent = null;
            _smokeTrail = null;
        }
    }

    public void FuckingExplode()
    {
        _explosionFX.AddComponent<TimedSelfDestruct>().maxLifeSpan = 5f;
        _explosionFX.transform.parent = null;
        _explosionFX.gameObject.SetActive(true);
        DetachSmokeTrail();
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
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //visualize tracking target
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(targetPos,  0.45f);
    }
}
