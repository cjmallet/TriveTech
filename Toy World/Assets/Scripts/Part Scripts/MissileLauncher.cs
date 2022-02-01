using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : OffensivePart
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _laserStart;
    [SerializeField] private Transform _rotatingLauncherPart;
    private bool _onCooldown;
    private float _timeSinceLaunch;
    private LineRenderer _lineRnd;


    // Start is called before the first frame update
    void Start()
    {
        _lineRnd = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if(gamestate playing)
        // {


        if (!_onCooldown)
        {
            DrawLaser();
        }
        else 
        {
            _timeSinceLaunch += Time.deltaTime;
            if (_timeSinceLaunch >= _cooldown)
                _onCooldown = false;
        }
        // }
    }

    void LaunchMissile()
    {

    }


    void DrawLaser()
    {
        _lineRnd.SetPosition(0, _laserStart.position);

        RaycastHit laserHit;
        if (Physics.Raycast(_laserStart.position, Camera.main.transform.position, out laserHit))
        {
            _lineRnd.SetPosition(1, laserHit.point);
            //laserImpactEffect.transform.position = laserHit.point;
        }
    }
}
