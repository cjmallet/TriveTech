using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject partSelectionCanvas, crossHair, buttonPrefab;
    [SerializeField] private GameObject selectedButton;

    //All lists for the UI categories
    private List<GameObject> movementParts, meleeParts, utilityParts, defenceParts, rangedParts, chassisParts;
    private List<GameObject> categoryHolders = new List<GameObject>();

    private int categoryIndex;
    private EventSystem eventSystem;
    //private GameObject selectedPart;

    /// <summary>
    /// Initialize the Part selection UI and the button listeners
    /// </summary>
    private void Start()
    {
        eventSystem = EventSystem.current;
        ChangeSelectedButton(selectedButton);

        foreach (ScrollRect categoryHolder in partSelectionCanvas.GetComponentsInChildren<ScrollRect>())
        {
            categoryHolders.Add(categoryHolder.gameObject);
        }

        movementParts = LoadParts("movementParts");
        foreach (GameObject part in movementParts)
        {
            CreateButton(part, "Movement");
        }

        meleeParts = LoadParts("meleeParts");
        foreach (GameObject part in meleeParts)
        {
            CreateButton(part, "Melee");
        }

        utilityParts = LoadParts("utilityParts");
        foreach (GameObject part in utilityParts)
        {
            CreateButton(part, "Utility");
        }

        defenceParts = LoadParts("defenceParts");
        foreach (GameObject part in defenceParts)
        {
            CreateButton(part, "Defence");
        }

        rangedParts = LoadParts("rangedParts");
        foreach (GameObject part in rangedParts)
        {
            CreateButton(part, "Ranged");
        }

        chassisParts = LoadParts("chassisParts");
        foreach (GameObject part in chassisParts)
        {
            CreateButton(part, "Chassis");
        }

        ChangeCategory("Chassis");
    }

    private void Update()
    {

    }

    private List<GameObject> LoadParts(string partName)
    {
        return Resources.LoadAll("Parts/"+partName, typeof(GameObject)).Cast<GameObject>().ToList();
    }

    private void CreateButton(GameObject part, string Category)
    {
        for (int x=0; x<categoryHolders.Count;x++)
        {
            if (categoryHolders[x].name.Contains(Category))
            {
                categoryIndex = x;
            }
        }

        GameObject newButton = Instantiate(buttonPrefab, categoryHolders[categoryIndex].GetComponentInChildren<GridLayoutGroup>().transform);
        newButton.name = part.name;
        newButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = part.name;
        newButton.GetComponent<Button>().onClick.AddListener(() => { ChangeSelectedPart(part); ClosePartSelectionUI(); VehicleEditor._instance.ChangeActiveBuildState(); });
    }

    public void BuildButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            VehicleEditor._instance.ChangeActiveBuildState();
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

    public void ChangeCategory(string categoryName)
    {
        foreach (GameObject category in categoryHolders)
        {
            if (category.name.Contains(categoryName))
            {
                category.SetActive(true);
            }
            else
            {
                category.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Close the Canvas of the part selection
    /// </summary>
    public void ClosePartSelectionUI()
    {
        partSelectionCanvas.SetActive(!partSelectionCanvas.activeSelf);
        crossHair.SetActive(!crossHair.activeSelf);
        FPSCameraControllers.canRotate = !FPSCameraControllers.canRotate;
        eventSystem.SetSelectedGameObject(selectedButton);
    }

    public void ChangeSelectedButton(GameObject button)
    {
        /*ColorBlock colors = selectedButton.GetComponent<Button>().colors;
        colors.normalColor= new Color(0, 166, 255, 255);
        selectedButton.GetComponent<Button>().colors = colors;
        /*selectedButton.GetComponent<Image>().color = new Color(191, 191, 191, 255);
        selectedButton.GetComponent<Button>().enabled = true;
    

        button.GetComponent<Image>().color = new Color(0, 166, 255, 255);
        button.GetComponent<Button>().enabled = false;
        */
        selectedButton = button;
    }
}
