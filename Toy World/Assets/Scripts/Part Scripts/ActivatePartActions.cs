using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivatePartActions : MonoBehaviour
{
    [HideInInspector]
    public List<Part> allParts;
    private List<OffensivePart> allOffensiveParts = new List<OffensivePart>();
    private List<DefensivePart> allDefensiveParts = new List<DefensivePart>();
    private List<UtilityPart> allUtilityParts = new List<UtilityPart>();

    private bool attackActionPressed,
        defenceActionPressed,
        sprintActionPressed,
        jumpActionPressed,
        utilityAction1Pressed,
        utilityAction2Pressed,
        utilityAction3Pressed,
        utilityAction4Pressed;

    /// <summary>
    /// Activates attack action for each offensive part on vehicle
    /// </summary>
    private void ActivateAttackActions()
    {
        foreach (OffensivePart offensivePart in allOffensiveParts)
        {
            offensivePart.AttackAction();
        }
    }

    /// <summary>
    /// Activates defence action for each defensivepart on vehicle
    /// </summary>
    private void ActivateDefenseActions()
    { 
        foreach (DefensivePart defensivePart in allDefensiveParts)
        {
            defensivePart.DefenceAction();
        }
    
    }

    /// <summary>
    /// Activates utility action based on specific input slot for each utility part on vehicle
    /// </summary>
    private void ActivateUtilityActions(UtilityPart.SpecificActionType specificActionType)
    {
        foreach (UtilityPart utilityPart in allUtilityParts)
        {
            if (utilityPart.specificActionType == specificActionType)
                utilityPart.UtilityAction();
        }
    }

    public void CategorizePartsInList()
    {
        foreach (Part part in allParts)
        {
            if (part is OffensivePart)
                allOffensiveParts.Add((OffensivePart)part);
            else if (part is DefensivePart)
                allDefensiveParts.Add((DefensivePart)part);
            else if (part is UtilityPart)
                allUtilityParts.Add((UtilityPart)part);
            else Debug.Log("Probably a movement part, do nothing...");
        }
    }

    /// <summary>
    /// Set specific utility action input slots for all utility parts on the vehicle
    /// </summary>
    public void SetSpecificActionType()
    {
        // Indication for which of 4 utility input slots should be used
        int specificUtilityActionSlot = 1;

        foreach (UtilityPart utilityPart in allUtilityParts)
        {
            switch (utilityPart.actionType)
            {
                // Sprint utility remains a sprint utility
                case UtilityPart.ActionType.Sprint:
                    utilityPart.specificActionType = UtilityPart.SpecificActionType.Sprint;
                    break;
                // Jump utility remains a jump utility
                case UtilityPart.ActionType.Jump:
                    utilityPart.specificActionType = UtilityPart.SpecificActionType.Jump;
                    break;
                // Other utility actions will get 1 of 4 input slots absed on index devined in first line of this function
                case UtilityPart.ActionType.Utility:
                    switch (specificUtilityActionSlot)
                    {
                        case 1:
                            utilityPart.specificActionType = UtilityPart.SpecificActionType.Utility1;
                            break;
                        case 2:
                            utilityPart.specificActionType = UtilityPart.SpecificActionType.Utility2;
                            break;
                        case 3:
                            utilityPart.specificActionType = UtilityPart.SpecificActionType.Utility3;
                            break;
                        case 4:
                            utilityPart.specificActionType = UtilityPart.SpecificActionType.Utility4;
                            break;
                        default:
                            Debug.Log("No valid utility action slot left!");
                            break;
                    }
                    // Action index goes to next slot, but when 4th slot is filled the action will be bounded to the first slot again
                    if (specificUtilityActionSlot < 4)
                        specificUtilityActionSlot++;
                    else
                        specificUtilityActionSlot = 1;
                    break;
                default:
                    Debug.Log("Not a valid action type!");
                    break;
            }
        }
    }

    public void UseAttackAction(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateAttackActions();
        if (value.canceled)
            ActivateAttackActions();
    }

    public void UseDefenceAction(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateDefenseActions();
        if (value.canceled)
            ActivateDefenseActions();
    }

    public void UseSprintAction(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Sprint);
        if (value.canceled)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Sprint);
    }

    public void UseJumpAction(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Jump);
        if (value.canceled)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Jump);
    }

    public void UseUtilityAction1(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility1);
        if (value.canceled)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility1);
    }

    public void UseUtilityAction2(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility2);
        if (value.canceled)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility2);
    }

    public void UseUtilityAction3(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility3);
        if (value.canceled)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility3);
    }

    public void UseUtilityAction4(InputAction.CallbackContext value)
    {
        if (value.started)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility4);
        if (value.canceled)
            ActivateUtilityActions(UtilityPart.SpecificActionType.Utility4);
    }
}
