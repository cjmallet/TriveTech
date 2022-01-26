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

    public void CategorizePartsInList(List<Part> partList)
    {
        allParts = partList;

        foreach (Part part in allParts)
        {
            if (part is OffensivePart)
                allOffensiveParts.Add((OffensivePart)part);
            else if (part is DefensivePart)
                allDefensiveParts.Add((DefensivePart)part);
            else if (part is UtilityPart)
                allUtilityParts.Add((UtilityPart)part);
            else
            {
                // do nothing
            }
        }

        SetSpecificActionType();
    }

    /// <summary>
    /// Resets all actions to be used as of they are used for the first time.
    /// Used for after resetting level.
    /// </summary>
    public void ResetAllActions()
    {
        foreach (OffensivePart part in allOffensiveParts)
        {
            part.ResetAction();
        }
        foreach (DefensivePart part in allDefensiveParts)
        {
            part.ResetAction();
        }
        foreach (UtilityPart part in allUtilityParts)
        {
            part.ResetAction();
        }
        allOffensiveParts.Clear();
        allDefensiveParts.Clear();
        allUtilityParts.Clear();
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
                // Utility parts with no action are set to default
                case UtilityPart.ActionType.None:
                    utilityPart.specificActionType = UtilityPart.SpecificActionType.Default;
                    break;
                // Sprint utility remains a sprint utility
                case UtilityPart.ActionType.Sprint:
                    utilityPart.specificActionType = UtilityPart.SpecificActionType.Sprint;
                    break;
                // Jump utility remains a jump utility
                case UtilityPart.ActionType.Jump:
                    utilityPart.specificActionType = UtilityPart.SpecificActionType.Jump;
                    break;
                // Other utility actions will get 1 of 4 input slots based on index devined in first line of this function
                // Or based on utility parts of same type
                case UtilityPart.ActionType.Utility:
                    bool hasInputSlot = false;

                    // Check if other parts of the same type are already assigned to an input. Yes? Take it as well!
                    foreach (UtilityPart actionCheckPart in allUtilityParts)
                    {
                        if (actionCheckPart.GetType() == utilityPart.GetType())
                        {
                            if (actionCheckPart.specificActionType != UtilityPart.SpecificActionType.Default)
                            {
                                utilityPart.specificActionType = actionCheckPart.specificActionType;
                                hasInputSlot = true;
                                break;
                            }
                        }
                    }
                    // If you are the first part of your kind. Get a new input slot
                    if (!hasInputSlot)
                    {
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
                    }
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

    public void UseDefenseAction(InputAction.CallbackContext value)
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
