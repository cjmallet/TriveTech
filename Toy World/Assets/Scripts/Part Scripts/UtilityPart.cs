using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityPart : Part
{
    // Direct reference to check if action button is being pressed
    public bool ActionButtonPressed { get; set; }

    /*
     * Use to specify when action should start/end 
     * (important to define toggle actions, one time action or hold button actions)
     */
    public bool DoAction { get; set; }

    public enum ActionType { Sprint, Jump, Utility }
    public ActionType actionType;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // UtilityAction();
    }

    public virtual void UtilityAction()
    {
    
    }

    public void SetCorrectInputAction()
    {
        switch (actionType)
        {
            case ActionType.Sprint:
                break;
            case ActionType.Jump:
                break;
            case ActionType.Utility:
                break;
            default:
                Debug.Log("Not a valid action type!");
                break;
        }
    }
}
