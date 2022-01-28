using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public CargoSpawner cargoSpawner;
    [SerializeField] private TriggerLevelStart levelTrigger;
    public int cargoCompletionAmount, cargoToSpawn;
    [SerializeField] private int timeLevelCompletion;
    [SerializeField] private GameObject levelUI;
    [SerializeField] private GameObject playerSpawn;

    private GameObject objectiveUI, panel, canvasText, timerObject;
    private float timer = 0;
    private bool timerStarted = false;

    public float timerBeforeCargoSpawn = 1;

    [HideInInspector] public int collectedCargo, displayCargoAmount;

    private void Start()
    {
        //set coreblock to spawn location
        DDOL.Instance.P1Coreblock.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
        DDOL.Instance.P1Coreblock.SetActive(true);
        panel = levelUI.transform.Find("Panel").gameObject;
        canvasText = levelUI.transform.Find("FinishText").gameObject;
        timerObject = levelUI.transform.Find("TimerObject").gameObject;
        objectiveUI = levelUI.transform.Find("Goals").gameObject;
        displayCargoAmount = cargoToSpawn;
    }

    private void FixedUpdate()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
            timerObject.GetComponent<TextMeshProUGUI>().text = ((int)(timeLevelCompletion - timer)).ToString();
        }

        if (timer > timeLevelCompletion)
        {
            timerStarted = false;
            OpenEndScreen("You ran out of time");
        }
    }

    private void OpenEndScreen(string completionMessage)
    {
        canvasText.GetComponent<TextMeshProUGUI>().text = completionMessage;
        canvasText.SetActive(true);
        panel.SetActive(true);
        objectiveUI.SetActive(false);
    }

    public void StartTimer()
    {
        timerStarted = true;
    }

    public void StopTimer()
    {
        timerStarted = false;
        timer = 0;
        levelTrigger.levelStarted = false;
        canvasText.SetActive(false);
        panel.SetActive(false);
        timerObject.GetComponent<TextMeshProUGUI>().text = "";
        cargoSpawner.ResetItems();
    }

    /// <summary>
    /// Start the level right away with some delay of spawning the cargo.
    /// </summary>
    public IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(timerBeforeCargoSpawn);
        cargoSpawner.SpawnItems();
    }

    public void LoseCargo()
    {
        collectedCargo--;
        displayCargoAmount--;
        objectiveUI.GetComponent<ObjectiveProgress>().UpdateCargo(displayCargoAmount);
    }

    public void ResetCargo()
    {
        displayCargoAmount = cargoToSpawn;
        objectiveUI.GetComponent<ObjectiveProgress>().UpdateCargo(displayCargoAmount);
    }

    public void FinishLevel()
    {
        timerStarted = false;

        if (timer < timeLevelCompletion)
        {
            if (collectedCargo >= cargoCompletionAmount)
            {
                AudioManager.Instance.Play(AudioManager.clips.LevelComplete, FindObjectOfType<CorePart>().GetComponent<AudioSource>());
                OpenEndScreen("You Finished!\nCargo:" + collectedCargo + "/" + cargoToSpawn + "\nTime left: " + (int)(timeLevelCompletion - timer));
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
            }
            else
            {
                OpenEndScreen("You lost too much cargo");
            }
        }
    }
}
