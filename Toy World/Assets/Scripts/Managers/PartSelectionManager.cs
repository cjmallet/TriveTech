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
    [SerializeField] private GameObject partSelectionCanvas, buttonPrefab;
    [SerializeField] private GameObject selectedButton;
    [SerializeField] private Gradient statGradient, weightGradient;
    public GameObject crossHair;
    public GameObject popupWindow, statUI;

    //All lists for the UI categories
    private List<GameObject> movementParts, offensiveParts, utilityParts, chassisParts;
    private List<GameObject> categoryHolders = new List<GameObject>();
    private GameObject toolTipWindow;

    private static float maxHealth = 15, maxWeight=15,maxAcceleration=225, maxSteering=30,maxBoostPower=3,maxJumpPower=3;

    private int categoryIndex;
    private EventSystem eventSystem;

    [HideInInspector]
    public bool buildUIOpen = true;

    /// <summary>
    /// Initialize the Part selection UI and the button listeners
    /// </summary>
    private void Start()
    {
        eventSystem = EventSystem.current;
        toolTipWindow = GameObject.Find("Controls");
        toolTipWindow.SetActive(false);
        ChangeSelectedButton(selectedButton);

        //Retrieve all the part categories that exist
        foreach (ScrollRect categoryHolder in partSelectionCanvas.GetComponentsInChildren<ScrollRect>())
        {
            categoryHolders.Add(categoryHolder.gameObject);
        }

        //Load all the part associated with the category and make a button for each one
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

        //Start with the chassis category being highlighted and opened
        ChangeCategory("Chassis");
    }

    /// <summary>
    /// Retrieve all the parts in the respective resource folder and save them to their list
    /// </summary>
    /// <param name="partName">The category of a part, like chassisParts</param>
    /// <returns></returns>
    private List<GameObject> LoadParts(string partName)
    {
        return Resources.LoadAll("Parts/"+partName, typeof(GameObject)).Cast<GameObject>().ToList();
    }

    /// <summary>
    /// Create a button in the part selection UI
    /// </summary>
    /// <param name="part">The part object that will be displayed</param>
    /// <param name="Category">The category in which the part is displayed</param>
    private void CreateButton(GameObject part, string Category)
    {
        for (int x=0; x<categoryHolders.Count;x++)
        {
            if (categoryHolders[x].name.Contains(Category))
            {
                categoryIndex = x;
            }
        }

        //Create the button and load the correct texture to display
        GameObject newButton = Instantiate(buttonPrefab, categoryHolders[categoryIndex].GetComponentInChildren<GridLayoutGroup>().transform);
        newButton.name = part.name;
        newButton.transform.GetComponentInChildren<RawImage>().texture = (Texture)Resources.Load("UI/Images/"+part.name);

        //Create all the listeners and triggers for the hovering stat window
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

        //Add all the required functionalities to the buttons
        if (part.name.Contains("Wheel") || part.name.Contains("Launcher"))

        {
            newButton.GetComponent<Button>().onClick.AddListener(() => { ChangeSelectedPart(part); ClosePartSelectionUI();
                ChangeActiveBuildState(); GameManager.Instance.vehicleEditor.ResetPreviewRotation(); AudioManager.Instance.MenuButtonClickSound(); });
        }
        else
        {
            newButton.GetComponent<Button>().onClick.AddListener(() => { ChangeSelectedPart(part); ClosePartSelectionUI(); ChangeActiveBuildState(); AudioManager.Instance.MenuButtonClickSound(); });

        }
    }

    /// <summary>
    /// What stats will be shown in the hover stat window
    /// </summary>
    /// <param name="partObject">The part displayed in the button</param>
    /// <param name="data">The pointer data for the button</param>
    public void OnHoverOverButton(GameObject partObject, PointerEventData data)
    {
        Part part = partObject.GetComponent<Part>();

        popupWindow.transform.position = data.position;

        //Data that can be set for each button
        for (int x=0; x<4;x++)
        {
            Transform statObject = popupWindow.transform.GetChild(x);

            //Set the UI objects to display the correct stats and activate them
            switch (x)
            {
                case 0:
                    statObject.GetComponent<TextMeshProUGUI>().text = part.description;
                    break;
                case 2:
                    statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health";

                    //Calculate the gradient fill amount and the color based on part stats
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.health / maxHealth);
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.health / maxHealth;
                    break;
                case 3:
                    statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight";

                    //Calculate the gradient fill amount and the color based on part stats
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = weightGradient.Evaluate(part.weight / maxWeight);
                    statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.weight / maxWeight;
                    break;
            }
            statObject.gameObject.SetActive(true);
        }

        //Data that can only be set for movementParts
        if (part is MovementPart)
        {
            for (int i = 2; i < 7; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                //Set the UI objects to display the correct stats and activate them
                switch (i)
                {
                    case 2:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.health / maxHealth);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.health / maxHealth;
                        break;
                    case 3:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weight";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = weightGradient.Evaluate((part.weight - 5) / maxWeight);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (part.weight - 5) / maxWeight;
                        break;
                    case 5:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Steering";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate((part.GetComponent<MovementPart>().steeringAngle-10) / maxSteering);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (part.GetComponent<MovementPart>().steeringAngle - 10) / 30f;
                        break;
                    case 6:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Acceleration";
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate((part.GetComponent<MovementPart>().maxTorgue - 25) / maxAcceleration);
                        statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (part.GetComponent<MovementPart>().maxTorgue - 25) / maxAcceleration;
                        break;
                }
                if (i!=4)
                {
                    statObject.gameObject.SetActive(true);
                }
            }
        }
        //Data that can only be set for booster and jump parts
        else if (part is BoostPart||part is JumpPart)
        {
            for (int i = 4; i < 5; i++)
            {
                Transform statObject = popupWindow.transform.GetChild(i);

                //Set the UI objects to display the correct stats and activate them
                switch (i)
                {
                    case 4:
                        statObject.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Power";
                        if (part is BoostPart)
                        {
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.GetComponent<BoostPart>().boostStrenght / maxBoostPower);
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.GetComponent<BoostPart>().boostStrenght / maxBoostPower;
                            break;
                        }
                        else
                        {
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = statGradient.Evaluate(part.GetComponent<JumpPart>().jumpStrenght / maxJumpPower);
                            statObject.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = part.GetComponent<JumpPart>().jumpStrenght / maxJumpPower;
                            break;
                        }
                        
                }
                statObject.gameObject.SetActive(true);
            }
        }

        if (!popupWindow.activeSelf)
            popupWindow.SetActive(true);
    }

    /// <summary>
    /// Close the part stat window UI when no longer hovering over button
    /// </summary>
    /// <param name="part">The part displayed in the button</param>
    /// <param name="data">The pointer data for the button</param>
    public void OnHoverExitButton(GameObject part, PointerEventData data)
    {
        for (int i = 1; i < popupWindow.transform.childCount; i++)
        {
            popupWindow.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (popupWindow.activeSelf)
            popupWindow.SetActive(false);
    }

    /// <summary>
    /// Switch the currently active UI and camera mode
    /// </summary>
    /// <param name="context"></param>
    public void BuildButton(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.Instance.stateManager.CurrentGameState == GameStateManager.GameState.Building)
        {
            ChangeActiveBuildState();

            if (popupWindow.activeSelf)
                popupWindow.SetActive(false);

            ClosePartSelectionUI();
        }
    }

    /// <summary>
    /// Enable or disable the part UI and the cursor mode
    /// </summary>
    public void ChangeActiveBuildState()
    {
        buildUIOpen = !buildUIOpen;
        toolTipWindow.SetActive(!buildUIOpen);
        if (buildUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// Change the selected part with the part connected to the button pressed
    /// </summary>
    /// <param name="chosenPart"></param>
    public void ChangeSelectedPart(GameObject chosenPart)
    {
        GameManager.Instance.vehicleEditor.SetSelectedPart(chosenPart);
    }

    /// <summary>
    /// Switch the active category displayed on the screen
    /// </summary>
    /// <param name="categoryName">The name of the category that will be displayed</param>
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

    /// <summary>
    /// Change the highlighted category button
    /// </summary>
    /// <param name="button">The button that was just pressed</param>
    public void ChangeSelectedButton(GameObject button)
    {
        selectedButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        selectedButton.GetComponent<Button>().enabled = true;
    
        button.GetComponent<Image>().color = new Color32(0, 166, 255, 255);
        button.GetComponent<Button>().enabled = false;
        
        selectedButton = button;
    }
}
