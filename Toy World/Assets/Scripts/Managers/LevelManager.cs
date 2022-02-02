using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager for all level specific triggers.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public CargoSpawner cargoSpawner;
    public int cargoCompletionAmount, cargoToSpawn;
    [SerializeField] private int timeLevelCompletion;
    [SerializeField] private GameObject levelUI;
    [SerializeField] private GameObject playerSpawn;

    private GameObject objectiveUI, endLevelScreen, timerObject;
    private float timer = 0;
    private bool timerStarted = false, hasBeenPlayed = false;

    public float timerBeforeCargoSpawn = 1;

    [HideInInspector] public int collectedCargo, displayCargoAmount;

    private void Start()
    {
        //set coreblock to spawn location
        DDOL.Instance.P1Coreblock.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
        DDOL.Instance.P1Coreblock.SetActive(true);
        endLevelScreen = levelUI.transform.Find("EndLevelScreen").gameObject;
        timerObject = levelUI.transform.Find("Timer").GetChild(0).gameObject;
        objectiveUI = levelUI.transform.Find("Goals").gameObject;
        displayCargoAmount = cargoToSpawn;
    }

    private void FixedUpdate()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
            timerObject.GetComponent<TextMeshProUGUI>().text = "Time left: " + ((int)(timeLevelCompletion - timer)).ToString();
        }

        if (timer > timeLevelCompletion)
        {
            timerStarted = false;
            OpenEndScreen("You ran out of time",0,0,false);

            if (!hasBeenPlayed)
            {
                AudioManager.Instance.Play(AudioManager.clips.GameOver, FindObjectOfType<EndLevel>().GetComponent<AudioSource>());
                AudioManager.Instance.Stop(AudioManager.Instance.musicSource2);
                hasBeenPlayed = true;
            }
        }
    }

    /// <summary>
    /// When the level ends, this opens the end screen.
    /// </summary>
    /// <param name="completionMessage">Succes or failure message.</param>
    /// <param name="time">Time left.</param>
    /// <param name="cargoDelivered">Amount of cargo delivered.</param>
    /// <param name="succes">Succesfull or not.</param>
    private void OpenEndScreen(string completionMessage,int time, int cargoDelivered,bool succes)
    {
        endLevelScreen.SetActive(true);
        endLevelScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = completionMessage;
        endLevelScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Time Left: "+time.ToString();
        endLevelScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Cargo: "+cargoDelivered.ToString() +" / "+ cargoToSpawn;
        endLevelScreen.transform.GetChild(6).gameObject.SetActive(succes);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.vehicleEditor.coreBlockPlayMode.GetComponent<PlayerInput>().enabled=false;
        GameManager.Instance.inputManager.enabled = false;
        objectiveUI.SetActive(false);
    }

    /// <summary>
    /// Starts the game timer.
    /// </summary>
    public void StartTimer()
    {
        timerStarted = true;
        timerObject.transform.parent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Start the level right away with some delay of spawning the cargo.
    /// </summary>
    public IEnumerator StartLevel()
    {
        GameManager.Instance.partSelectionManager.toolTipWindow.SetActive(false);
        yield return new WaitForSeconds(timerBeforeCargoSpawn);
        cargoSpawner.SpawnItems();
    }

    /// <summary>
    /// Called when cargo is dropped on the floor.
    /// </summary>
    public void LoseCargo()
    {
        collectedCargo--;
        displayCargoAmount--;
        objectiveUI.GetComponent<ObjectiveProgress>().UpdateCargo(displayCargoAmount);
    }

    /// <summary>
    /// Resets all cargo in the level.
    /// </summary>
    public void ResetCargo()
    {
        displayCargoAmount = cargoToSpawn;
        objectiveUI.GetComponent<ObjectiveProgress>().UpdateCargo(displayCargoAmount);
    }

    /// <summary>
    /// Finishes the level when the player crosses the gate.
    /// </summary>
    public void FinishLevel()
    {
        timerStarted = false;

        if (timer < timeLevelCompletion)
        {
            if (collectedCargo >= cargoCompletionAmount)
            {
                AudioManager.Instance.Play(AudioManager.clips.LevelComplete, GameObject.FindGameObjectWithTag("DeliveryPort").GetComponent<AudioSource>());
                AudioManager.Instance.Stop(AudioManager.Instance.musicSource2);
                OpenEndScreen("You Finished!", (int)(timeLevelCompletion - timer), collectedCargo,true);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
                if (collectedCargo==cargoToSpawn)
                {
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Cleared", 1);
                }
            }
            else
            {
                AudioManager.Instance.Play(AudioManager.clips.GameOver, GameObject.FindGameObjectWithTag("DeliveryPort").GetComponent<AudioSource>());
                AudioManager.Instance.Stop(AudioManager.Instance.musicSource2);
                OpenEndScreen("You lost too much cargo", (int)(timeLevelCompletion - timer), collectedCargo,false);
            }
        }
    }
}
