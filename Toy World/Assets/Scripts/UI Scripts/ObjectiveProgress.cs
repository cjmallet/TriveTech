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
        levelManager = LevelManager.Instance;
        currentCargo = levelManager.cargoSpawner.cargoToSpawn;
        mainObjectiveTracker = transform.Find("MainGoalProgress").GetComponent<TextMeshProUGUI>();

        transform.Find("MainGoal").GetComponent<TextMeshProUGUI>().text = "Deliver at least "+ levelManager.cargoCompletionAmount+ " pieces of wood";
        mainObjectiveTracker.text = currentCargo + " / " + currentCargo;
    }

    public void LoseCargo()
    {
        currentCargo--;
        mainObjectiveTracker.text = currentCargo + " / " + levelManager.cargoSpawner.cargoToSpawn;
    }
}
