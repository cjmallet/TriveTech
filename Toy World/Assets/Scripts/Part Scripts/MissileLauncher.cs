using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float _lookAngleCorrection = -25f;


    void Start()
    {
        _lineRnd = GetComponent<LineRenderer>();
        _vehicleCam = GameManager.Instance.vehicleEditor.vehicleCam.transform;
        //prepare missile prefab with values that won't change
        _missilePrefab = Resources.Load("Missile") as GameObject;
    }

    // Update is called once per frame
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

    public override void AttackAction()
    {
        if (!_onCooldown)
            LaunchMissile();
    }

    private void LaunchMissile()
    {
        //spawn missile
        HomingMissile missile = Instantiate(_missilePrefab, _missileProp.transform.position, _missileProp.transform.rotation).GetComponent<HomingMissile>();
        missile.launcher = gameObject;
        missile.transform.localScale = _missileProp.transform.lossyScale;
        RaycastHit laserHit;
        //correct angle, because you'll be looking slightly down on your vehicle, not straight from behind
        Vector3 direction = Quaternion.AngleAxis(_lookAngleCorrection, new Vector3(1, 0f, 0f)) * _vehicleCam.forward;

        if (Physics.Raycast(_laserStart.position, direction, out laserHit, Mathf.Infinity, ~ignoreLayers))
        {
            missile.target = laserHit.point;
        }
        else
        {
            missile.target = Vector3.zero;
        }
        SetCooldown(true);
    }

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

    void DrawLaser()
    {
        _lineRnd.SetPosition(0, _laserStart.position);
        //correct angle, because you'll be looking slightly down on your vehicle, not straight from behind
        Vector3 direction = Quaternion.AngleAxis(_lookAngleCorrection, new Vector3(1, 0f, 0f)) * _vehicleCam.forward;
        RaycastHit laserHit;
        if (Physics.Raycast(_laserStart.position, direction, out laserHit, Mathf.Infinity, ~ignoreLayers))
        {
            _lineRnd.SetPosition(1, laserHit.point);
            //laserImpactEffect.transform.position = laserHit.point;
        }
        else
        {
            _lineRnd.SetPosition(1, _rotatingLauncherPart.forward * 100f);
        }
            
    }
}
