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
    private static PartSelectionManager instance;
    public static PartSelectionManager _instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private GameObject partSelectionCanvas, buttonPrefab;
    [SerializeField] private GameObject selectedButton;
    public GameObject crossHair;
    public GameObject popupWindow, statUI;

    //All lists for the UI categories
    private List<GameObject> movementParts, meleeParts, utilityParts, defenceParts, rangedParts, chassisParts;
    private List<GameObject> categoryHolders = new List<GameObject>();

    private int categoryIndex;
    private EventSystem eventSystem;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

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
        newButton.transform.GetComponentInChildren<RawImage>().texture = (Texture)Resources.Load("UI/Images/"+part.name);

        EventTrigger trigger = newButton.AddComponent<EventTrigger>();
        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        EventTrigger.Entry clickEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        exitEvent.eventID = EventTriggerType.PointerExit;
        clickEvent.eventID = EventTriggerType.PointerClick;
        enterEvent.callback.AddListener((data) => { OnHoverOverButton(part, (PointerEventData)data); });
        exitEvent.callback.AddListener((data) => { OnHoverExitButton(part, (PointerEventData)data); });
        clickEvent.callback.AddListener((data) => { OnHoverExitButton(part, (PointerEventData)data); });
        trigger.triggers.Add(enterEvent);
        trigger.triggers.Add(exitEvent);
        trigger.triggers.Add(clickEvent);

        if (part.name == "Wheel")
        {
            newButton.GetComponent<Button>().onClick.AddListener(() => { ChangeSelectedPart(part); ClosePartSelectionUI(); 
                VehicleEditor._instance.ChangeActiveBuildState(); VehicleEditor._instance.ResetPreviewRotation();});
        }
        else
        {
            newButton.GetComponent<Button>().onClick.AddListener(() => { ChangeSelectedPart(part); ClosePartSelectionUI(); VehicleEditor._instance.ChangeActiveBuildState(); });
        }
    }

    public void OnHoverOverButton(GameObject partObject, PointerEventData data)
    {
        Part part = partObject.GetComponent<Part>();

        popupWindow.transform.position = data.position;

        if (part is MovementPart)
        {
            for (int i = 1; i < 5; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                switch (i)
                {
                    case 1:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health: " + part.health;
                        break;
                    case 2:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight: " + part.weight;
                        break;
                    case 3:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Torque: " + part.GetComponent<MovementPart>().maxTorgue;
                        break;
                    case 4:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Steering: " + part.GetComponent<MovementPart>().steeringAngle;
                        break;
                }

                statObject.gameObject.SetActive(true);
            }
        }
        else if (part is BoostPart) // Im aware this is double, but that's because more stats might be added.
        {
            for (int i = 1; i < 4; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                switch (i)
                {
                    case 1:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health: " + part.health;
                        break;
                    case 2:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight: " + part.weight;
                        break;
                    case 3:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Boost pwr: " + part.GetComponent<BoostPart>().boostStrenght;
                        break;
                }

                statObject.gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 1; i < 3; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                switch (i)
                {
                    case 1:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health: " + part.health;
                        break;
                    case 2:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight: " + part.weight;
                        break;
                }

                statObject.gameObject.SetActive(true);
            }
        }
        
        if (!popupWindow.activeSelf)
            popupWindow.SetActive(true);
    }

    public void OnHoverExitButton(GameObject part, PointerEventData data)
    {
        for (int i = 1; i < popupWindow.transform.childCount; i++)
        {
            popupWindow.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (popupWindow.activeSelf)
            popupWindow.SetActive(false);
    }

    public void BuildButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            VehicleEditor._instance.ChangeActiveBuildState();

            if (popupWindow.activeSelf)
                popupWindow.SetActive(false);

            ClosePartSelectionUI();
        }
    }

    /// <summary>
    /// Change the selected part with the part connected to the button pressed
    /// </summary>
    /// <param name="chosenPart"></param>
    public void ChangeSelectedPart(GameObject chosenPart)
    {
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
        FPSCameraControllers.canRotate = !FPSCameraControllers.canRotate;
        crossHair.SetActive(!crossHair.activeSelf);
        eventSystem.SetSelectedGameObject(selectedButton);
    }

    public void ChangeSelectedButton(GameObject button)
    {
        selectedButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        selectedButton.GetComponent<Button>().enabled = true;
    
        button.GetComponent<Image>().color = new Color32(0, 166, 255, 255);
        button.GetComponent<Button>().enabled = false;
        
        selectedButton = button;
    }
}
