using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FartBoost : UtilityPart
{
    private void FixedUpdate()
    {
        //UtilityAction();
    }

    public override void UtilityAction()
    {
        if (!DoAction)
        {
            Debug.Log("Fart!");
            DoAction = true;
        }
        else
            DoAction = false;
    } 
}
