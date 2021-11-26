using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject PartSelectionCanvas, ButtonPrefab, contentHolder;
    [SerializeField] private List<GameObject> parts;

    //private GameObject selectedPart;

    /// <summary>
    /// Initialize the Part selection UI and the button listeners
    /// </summary>
    private void Start()
    {
        foreach (GameObject part in parts)
        {
            GameObject newButton=Instantiate(ButtonPrefab,contentHolder.transform);
            newButton.name = part.name;
            newButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = part.name;
            newButton.GetComponent<Button>().onClick.AddListener(() => { ChangeSelectedPart(part);});
        }
    }

    /// <summary>
    /// Check if the button to close the canvas is pressed
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ClosePartSelectionUI();
        }
    }

    /// <summary>
    /// Change the selected part with the part connected to the button pressed
    /// </summary>
    /// <param name="chosenPart"></param>
    public void ChangeSelectedPart(GameObject chosenPart)
    {
        //selectedPart = chosenPart;
        VehicleEditor._instance.SetSelectedPart(chosenPart);
    }

    /// <summary>
    /// Close the Canvas of the part selection
    /// </summary>
    public void ClosePartSelectionUI()
    {
        PartSelectionCanvas.SetActive(!PartSelectionCanvas.activeSelf);
    }
}
