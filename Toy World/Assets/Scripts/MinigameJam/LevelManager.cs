using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get { return instance; }
    }

    public CargoSpawner cargoSpawner;
    public int cargoCompletionAmount;

    [SerializeField] private int timeLevelCompletion;
    [SerializeField] private GameObject levelUI;

    private GameObject objectiveUI,panel,canvasText,timerObject;
    private float timer = 0;
    private bool timerStarted = false;

    [HideInInspector]public int collectedCargo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        panel = levelUI.transform.Find("Panel").gameObject;
        canvasText = levelUI.transform.Find("FinishText").gameObject;
        timerObject = levelUI.transform.Find("TimerObject").gameObject;
        objectiveUI = levelUI.transform.Find("Goals").gameObject;
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

    public void StartLevel()
    {
        cargoSpawner.SpawnItems();
    }

    public void LoseCargo()
    {
        collectedCargo--;
        objectiveUI.GetComponent<ObjectiveProgress>().LoseCargo();
    }

    public void FinishLevel()
    {
        timerStarted = false;

        if (timer < timeLevelCompletion)
        {
            if (collectedCargo >= cargoCompletionAmount)
            {
                OpenEndScreen("You Finished!\nCargo:" + collectedCargo + "/10 \nTime left: " + (int)(timeLevelCompletion - timer));
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name,1);
            }
            else
            {
                OpenEndScreen("You lost too much cargo");
            }
        }
    }
}
