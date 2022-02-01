using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveProgress : MonoBehaviour
{
    private LevelManager levelManager;
    private TextMeshProUGUI mainObjectiveTracker;
    private int currentCargo;

    /// <summary>
    /// Initialize the main goal and the UI
    /// </summary>
    void Start()
    {
        levelManager = GameManager.Instance.levelManager;
        currentCargo = levelManager.cargoToSpawn;
        mainObjectiveTracker = transform.Find("MainGoalProgress").GetComponent<TextMeshProUGUI>();

        transform.Find("MainGoal").GetComponent<TextMeshProUGUI>().text = "Deliver at least "+ levelManager.cargoCompletionAmount+ " pieces of wood";
        mainObjectiveTracker.text = currentCargo + " / " + currentCargo;
    }

    /// <summary>
    /// Update the cargo amount shown in the main obective
    /// </summary>
    /// <param name="cargoAmount"></param>
    public void UpdateCargo(int cargoAmount)
    {
        mainObjectiveTracker.text = cargoAmount + " / " + levelManager.cargoToSpawn;
    }
}
