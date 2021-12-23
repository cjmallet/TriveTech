using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingPart : UtilityPart
{
    [SerializeField]
    private GameObject lightBulb;

    private bool activateLight = true;

    /// <summary>
    /// Do switchlights with a toggle through 'DoAction' 
    /// </summary>
    public override void UtilityAction()
    {
        if (!DoAction)
        {
            LightSwitch();
            DoAction = true;
        }
        else DoAction = false;
    }

    /// <summary>
    /// Switch lights on or off
    /// </summary>
    private void LightSwitch()
    {
        activateLight = !activateLight;
        lightBulb.SetActive(activateLight);
    }
}
