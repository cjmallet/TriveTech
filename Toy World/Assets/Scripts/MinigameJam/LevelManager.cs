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
    [SerializeField] private int timeLevelCompletion;
    [SerializeField] private GameObject canvasText, panel, timerObject, playerSpawn;

    private float timer = 0;
    private bool timerStarted = false;

    public int collectedCargo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        //set coreblock to spawn location
        DDOL.Instance.P1Coreblock.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
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
            canvasText.GetComponent<TextMeshProUGUI>().text = "You ran out of time";
            canvasText.SetActive(true);
            panel.SetActive(true);
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
    }

    public void StartLevel()
    {
        cargoSpawner.SpawnItems();
    }

    public void FinishLevel()
    {
        timerStarted = false;

        if (timer < timeLevelCompletion)
        {
            if (collectedCargo > 2)
            {
                canvasText.GetComponent<TextMeshProUGUI>().text = "You Finished!\tCargo:" + collectedCargo + "/10 \tTime left: " + (int)(timeLevelCompletion - timer);
                canvasText.SetActive(true);
                panel.SetActive(true);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name,1);
            }
            else
            {
                canvasText.GetComponent<TextMeshProUGUI>().text = "You lost too much cargo";
                canvasText.SetActive(true);
                panel.SetActive(true);
            }
        }
    }
}
