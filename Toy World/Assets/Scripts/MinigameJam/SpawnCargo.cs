using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnCargo : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab,cargoSpawnPoint, timerObject, canvasText,panel;
    [SerializeField] private int cargoToSpawn;
    [SerializeField] private int timeLevelCompletion;

    private List<GameObject> cargo= new List<GameObject>();
    private float waitTime = 0.2f,timer,itemsSpawned;
    private bool underSpawn,finished;

    // Update is called once per frame
    void Update()
    {
        if (underSpawn&&!finished)
        {
            timer += Time.deltaTime;
            timerObject.GetComponent<TextMeshProUGUI>().text = ((int)(timeLevelCompletion-timer)).ToString();
        }
        
        if (underSpawn && timer>waitTime &&itemsSpawned<cargoToSpawn)
        {
            timer = 0;
            itemsSpawned++;
            GameObject wood=Instantiate(cargoPrefab, cargoSpawnPoint.transform);
            cargo.Add(wood);
        }

        if (timer>timeLevelCompletion)
        {
            canvasText.GetComponent<TextMeshProUGUI>().text = "You ran out of time";
            canvasText.SetActive(true);
            panel.SetActive(true);
            finished=true;
        }
    }

    public void SpawnItems()
    {
        underSpawn = true;
    }

    public void Finish()
    {
        finished = true;
        if (timer < timeLevelCompletion)
        {
            int collectedCargo=0;
            foreach (GameObject wood in cargo)
            {
                if (!wood.GetComponent<WoodCollider>().collisionName.Contains("Plane"))
                {
                    collectedCargo++;
                }
            }

            if (collectedCargo>2)
            {
                canvasText.GetComponent<TextMeshProUGUI>().text = "You Finished!\tCargo:" + collectedCargo + "/10 \tTime left: " + (int)(timeLevelCompletion - timer);
                canvasText.SetActive(true);
                panel.SetActive(true);
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
