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
    [SerializeField] private bool _addRandomWobble;//enable or disable altogether
    [SerializeField] private float _perlinMultiplier;//size/strength of the noise
    [SerializeField] private float _perlinScale;//how "smooth" the noise will be
    private Vector3 _seed;
    private Vector3 targetPos;
    private bool tracking;
    private bool collisionEnabled = false;


    // Use this for initialization
    void Start()
    {
        //each rocket gets it's own seed so 2 missiles will never fly the same path
        _seed = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
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
        Debug.Log(collision.collider.transform.name);
        FuckingExplode();
    }

    private void HomeInOnTarget()
    {
        //give randomness to the target position for that cute wobble during flight, in the form of perlin noise, based on it's location
        targetPos = target 
            + GetPerlinValues()
            * Vector3.Distance(target, transform.position)
            * 0.05f;

        //add velocity. rockets go woosh
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + transform.up * _force, _maxSpeed);

        //Rotate by setting angular velocity
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, (targetPos - transform.position).normalized);
        float angleInDegrees;
        Vector3 rotationAxis;
        targetRot.ToAngleAxis(out angleInDegrees, out rotationAxis);
        Vector3 angularDisplacement = rotationAxis * angleInDegrees * Mathf.Deg2Rad;

        _rb.angularVelocity = 
            _rb.angularVelocity 
            + angularDisplacement 
            * angularDisplacement.magnitude
            * _rotationForce
            * 0.1f;
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
        Debug.Log("is there a launcher? " + launcher);
        if(!launcher || Vector3.Distance(transform.position, launcher.transform.position) > 1.5f)
        {
            GetComponent<Collider>().enabled = true;
            collisionEnabled = true;
        }
    }

    Vector3 GetPerlinValues()
    {
        Vector3 perlin = Vector3.zero;
        if (_addRandomWobble)
        {
            Vector3 pos = (transform.position + _seed) * _perlinScale;
            perlin = new Vector3(
                Mathf.PerlinNoise(pos.y, pos.z) * 2 - 1,
                Mathf.PerlinNoise(pos.x, pos.z) * 2 - 1,
                Mathf.PerlinNoise(pos.x, pos.y) * 2 - 1
                );
            perlin *= perlin.magnitude * _perlinMultiplier;
            
        }
        return perlin;
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
            if(TryGetComponent(out ShootingEnemy enemy))
            {
                //rek enemy. hier is nog geen functie voor
            }
        } 
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //visualize tracking target
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPos,  0.45f);
    }
}
