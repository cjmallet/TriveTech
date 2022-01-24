using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveProgress : MonoBehaviour
{
    private LevelManager levelManager;
    private TextMeshProUGUI mainObjectiveTracker;
    private int currentCargo;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameManager.Instance.levelManager;
        currentCargo = levelManager.cargoToSpawn;
        mainObjectiveTracker = transform.Find("MainGoalProgress").GetComponent<TextMeshProUGUI>();

        transform.Find("MainGoal").GetComponent<TextMeshProUGUI>().text = "Deliver at least "+ levelManager.cargoCompletionAmount+ " pieces of wood";
        mainObjectiveTracker.text = currentCargo + " / " + currentCargo;
    }

    public void UpdateCargo(int cargoAmount)
    {
        mainObjectiveTracker.text = cargoAmount + " / " + levelManager.cargoToSpawn;
    }
}
