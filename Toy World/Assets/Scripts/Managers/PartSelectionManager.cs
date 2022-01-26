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
    [SerializeField] private Gradient statGradient, weightGradient;
    public GameObject crossHair;
    public GameObject popupWindow, statUI;

    //All lists for the UI categories
    private List<GameObject> movementParts, offensiveParts, utilityParts, chassisParts;
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

        offensiveParts = LoadParts("offensiveParts");
        foreach (GameObject part in offensiveParts)
        {
            CreateButton(part, "Offensive");
        }

        utilityParts = LoadParts("utilityParts");
        foreach (GameObject part in utilityParts)
        {
            CreateButton(part, "Utility");
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

        if (part.name.Contains("Wheel"))
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

        for (int x=0; x<4;x++)
        {
            Transform statObject = popupWindow.transform.GetChild(x);

            switch (x)
            {
                case 0:
                    statObject.GetComponent<TextMeshProUGUI>().text = part.description;
                    break;
                case 2:
                    statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health";
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.health / 15f);
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.health / 15f;
                    break;
                case 3:
                    statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight";
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = weightGradient.Evaluate(part.weight / 15f);
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.weight / 15f;
                    break;
            }
            statObject.gameObject.SetActive(true);
        }

        if (part is MovementPart)
        {
            for (int i = 2; i < 7; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                switch (i)
                {
                    case 2:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.health / 15f);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.health / 15f;
                        break;
                    case 3:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = weightGradient.Evaluate((part.weight - 5) / 15f);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (part.weight - 5) / 15f;
                        break;
                    case 5:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Steering";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate((part.GetComponent<MovementPart>().steeringAngle-10) / 30f);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (part.GetComponent<MovementPart>().steeringAngle - 10) / 30f;
                        break;
                    case 6:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Acceleration";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate((part.GetComponent<MovementPart>().maxTorgue - 25) / 225f);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (part.GetComponent<MovementPart>().maxTorgue - 25) / 225f;
                        break;
                }
                if (i!=4)
                {
                    statObject.gameObject.SetActive(true);
                }
            }
        }
        else if (part is BoostPart||part is JumpPart) // Im aware this is double, but that's because more stats might be added.
        {
            for (int i = 4; i < 5; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                switch (i)
                {
                    case 4:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Power";
                        if (part is BoostPart)
                        {
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.GetComponent<BoostPart>().boostStrenght / 3f);
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.GetComponent<BoostPart>().boostStrenght / 3f;
                            break;
                        }
                        else
                        {
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.GetComponent<JumpPart>().jumpStrenght / 3f);
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.GetComponent<JumpPart>().jumpStrenght / 3f;
                            break;
                        }
                        
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
