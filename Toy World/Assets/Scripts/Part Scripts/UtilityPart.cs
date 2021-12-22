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

    // Action type you can adjust per part in inspector/prefab
    public enum ActionType { Sprint, Jump, Utility }
    public ActionType actionType;

    // Actual action type given in runtime to determine which input slot a utility action responds to
    public enum SpecificActionType { Sprint, Jump, Utility1, Utility2, Utility3, Utility4 }
    [HideInInspector]
    public SpecificActionType specificActionType;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    public virtual void UtilityAction()
    {

    }
}
