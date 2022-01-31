using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : OffensivePart
{
    [SerializeField] private float _cooldown;
    private bool _onCooldown;
    private float _timeSinceLaunch;


    // Start is called before the first frame update
    void Start()
    {
        
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


    void DrawLaser()
    {

    }
}
