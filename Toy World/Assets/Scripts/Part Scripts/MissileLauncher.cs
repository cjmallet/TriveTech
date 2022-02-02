using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The missile launcher rotates and aims using the vehicle camera during play mode and launches missiles to destroy enemies and destructable objects.
/// By Ruben de Graaf.
/// </summary>
public class MissileLauncher : OffensivePart
{
    public LayerMask ignoreLayers;
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _laserStart;
    [SerializeField] private Transform _rotatingLauncherPart;
    [SerializeField] private GameObject _missileProp;
    private bool _onCooldown;
    private float _timeSinceLaunch;
    private LineRenderer _lineRnd;
    private Transform _vehicleCam;
    private GameObject _missilePrefab;
    private float _lookAngleCorrection = -30f;


    void Start()
    {
        _lineRnd = GetComponent<LineRenderer>();
        _vehicleCam = GameManager.Instance.vehicleEditor.vehicleCam.transform;
        //prepare missile prefab with values that won't change
        _missilePrefab = Resources.Load("Missile") as GameObject;
    }

    
    void FixedUpdate()
    {
        if (GameManager.Instance.stateManager.CurrentGameState == GameStateManager.GameState.Playing)
        {
            Vector3 forward = _vehicleCam.forward;
            forward.y = 0f;
            _rotatingLauncherPart.transform.rotation = Quaternion.LookRotation(forward, transform.up);

            if (!_onCooldown)
            {
                DrawLaser();
            }
            else
            {
                _timeSinceLaunch += Time.deltaTime;
                if (_timeSinceLaunch >= _cooldown)
                    SetCooldown(false);
            }
        }
    }

    /// <summary>
    /// Overrides the AttackAction from base class OffensivePart to call 
    /// LaunchMissile() when the attack button is pressed and it's not _onCooldown.
    /// </summary>
    public override void AttackAction()
    {
        if (!_onCooldown)
            LaunchMissile();
    }

    /// <summary>
    /// Instantiates the missile prefab and sets it's target to a raycast point, based on camera direction. Then resets the cooldown.
    /// </summary>
    private void LaunchMissile()
    {
        //spawn missile
        HomingMissile missile = Instantiate(_missilePrefab, _missileProp.transform.position, _missileProp.transform.rotation).GetComponent<HomingMissile>();
        missile.launcher = gameObject;
        missile.transform.localScale = _missileProp.transform.lossyScale;
        RaycastHit laserHit;
        //correct angle, because you'll be looking slightly down on your vehicle, not straight from behind
        Vector3 direction = Quaternion.AngleAxis(_lookAngleCorrection, _vehicleCam.right) * _vehicleCam.forward;

        if (Physics.Raycast(_laserStart.position, direction, out laserHit, Mathf.Infinity, ~ignoreLayers, QueryTriggerInteraction.Ignore))
        {
            missile.target = laserHit.point;
        }
        else
        {
            missile.target = Vector3.zero;
        }
        SetCooldown(true);
    }

    /// <summary>
    /// Used to set the cooldown on or off. This shows or hides the missile model and the laser pointer on the launcher as an indicator.
    /// </summary>
    /// <param name="state">Pass true if the missile fired and the launcher is on cooldown. False if it's ready to fire again.</param>
    void SetCooldown(bool state)
    {
        if (state)
        {
            _onCooldown = true;
            _timeSinceLaunch = 0f;
            _missileProp.SetActive(false);
            _lineRnd.enabled = false;
            _timeSinceLaunch = 0f;
        }
        else
        {
            _lineRnd.enabled = true;
            _onCooldown = false;
            _missileProp.SetActive(true);
        }
    }

    /// <summary>
    /// This draws a line using the attached LineRenderer component between the attached _laserStart transform and whatever the raycast hits.
    /// The raycast is the same as in the LaunchMissile() method (I should've made it a seperate method, welp). 
    /// If the raycast doesn't hit anything, the laser just aims straight forward over the Z axis.
    /// </summary>
    void DrawLaser()
    {
        _lineRnd.SetPosition(0, _laserStart.position);
        //correct angle, because you'll be looking slightly down on your vehicle, not straight from behind
        Vector3 direction = Quaternion.AngleAxis(_lookAngleCorrection, _vehicleCam.right) * _vehicleCam.forward;
        RaycastHit laserHit;

        if (Physics.Raycast(_laserStart.position, direction, out laserHit, Mathf.Infinity, ~ignoreLayers, QueryTriggerInteraction.Ignore))
        {
            _lineRnd.SetPosition(1, laserHit.point);
            //laserImpactEffect.transform.position = laserHit.point;
        }
        else
        {
            _lineRnd.SetPosition(1, transform.position + _rotatingLauncherPart.forward  * 100f);
        }
            
    }
}
