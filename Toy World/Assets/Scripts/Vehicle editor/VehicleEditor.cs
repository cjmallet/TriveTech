using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;

public class VehicleEditor : MonoBehaviour
{
    private static VehicleEditor instance;
    public static VehicleEditor _instance
    {
        get
        {
            return instance;
        }
    }

    public Quaternion partRotation;
    [SerializeField] private GameObject coreBlock;
    public GameObject coreBlockPlayMode;
    [SerializeField] private GameObject selectedPart;
    private PartGrid partGrid;

    private GameObject previewedPart;
    public bool playan, buildUIOpen = true;
    private Camera mainCam;

    private int vCount;

    public Camera vehicleCam;

    [SerializeField]
    private PlayerInput playerInput;

    public TextMeshProUGUI RestartText;

    private GameObject statWindow;

    private List<Part> parts = new List<Part>();

    void Awake()
    {
        //set the static instance
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    void Start()
    {
        coreBlock = DDOL.Instance.P1Coreblock;
        coreBlock.SetActive(true);
        partGrid = coreBlock.GetComponent<PartGrid>();
        statWindow = GameObject.Find("StatWindow");
        playerInput.SwitchCurrentActionMap("UI");
        SetSelectedPart(selectedPart);
        mainCam = Camera.main;
        if (vehicleCam == null)
        {
            vehicleCam = coreBlock.GetComponentInChildren<Camera>();
        }

        statWindow.GetComponent<StatWindowUI>().allParts = partGrid.ReturnAllParts();
        statWindow.GetComponent<StatWindowUI>().SetupAllParts();
    }

    public void Play()
    {
        if (!playan)
        {
            Destroy(previewedPart);
            //make a copy of the coreblock for playmode
            coreBlockPlayMode = Instantiate(coreBlock, coreBlock.transform.position, coreBlock.transform.rotation);

            // Clear list of parts if it still has parts
            if (coreBlockPlayMode.GetComponent<VehicleMovement>().wheelInfos.Count != 0)
                coreBlockPlayMode.GetComponent<VehicleMovement>().wheelInfos.Clear();

            coreBlockPlayMode.GetComponent<PartGrid>().RemakePartGrid();
            coreBlockPlayMode.GetComponent<PartGrid>().ToggleBoundingBox(false);

            // Fill parts lists needed for other scripts
            if (parts.Count != 0)
                parts.Clear();

            parts = coreBlock.GetComponent<PartGrid>().ReturnAllParts();
            parts = coreBlockPlayMode.GetComponent<PartGrid>().ReturnAllParts();
            coreBlockPlayMode.GetComponent<VehicleMovement>().allParts = parts;
            statWindow.GetComponent<StatWindowUI>().allParts = parts;
            coreBlockPlayMode.GetComponent<ActivatePartActions>().allParts = parts;
            coreBlockPlayMode.GetComponent<ActivatePartActions>().CategorizePartsInList();
            coreBlockPlayMode.GetComponent<ActivatePartActions>().SetSpecificActionType();

            coreBlockPlayMode.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            coreBlockPlayMode.GetComponent<Rigidbody>().mass = 0f;
            coreBlockPlayMode.GetComponent<Rigidbody>().drag = 0.5f;
            // Remove direction indication
            foreach (Part vehiclePart in parts)
            {
                // Fill list with movement parts for movement script
                if (vehiclePart is MovementPart)
                {
                    if (vehiclePart.transform.position.z > 0)
                        vehiclePart.GetComponent<MovementPart>().frontPart = true;
                    else
                        vehiclePart.GetComponent<MovementPart>().frontPart = false;
                    coreBlockPlayMode.GetComponent<VehicleMovement>().AddWheel((MovementPart)vehiclePart);
                }
                vehiclePart.SwitchColliders();

                // Remove direction indication
                if (vehiclePart.useDirectionIndicator)
                {
                    vehiclePart.ToggleDirectionIndicator(false);
                }
            }
            EscMenuBehaviour.buildCameraPositionStart = mainCam.transform.position;
            EscMenuBehaviour.buildCameraRotationStart = mainCam.transform.rotation;

            coreBlockPlayMode.GetComponent<VehicleMovement>().enabled = true;
            coreBlockPlayMode.GetComponent<ActivatePartActions>().enabled = true;

            coreBlockPlayMode.GetComponent<PartGrid>().CheckConnection();

            mainCam.gameObject.SetActive(false);
            //grab vehiclecam again, because we have a new coreblock instance for playmode
            vehicleCam = coreBlockPlayMode.GetComponentInChildren<Camera>();
            vehicleCam.enabled = true;
            playan = true;

            coreBlock.SetActive(false);//OG coreblock is disabled until back in build mode


            if (buildUIOpen)
            {
                PartSelectionManager._instance.ClosePartSelectionUI();
                ChangeActiveBuildState();
            }

            if (statWindow == null)
            {
                statWindow = GameObject.Find("StatWindow");
                statWindow.SetActive(false);
            }
            else
                statWindow.SetActive(false);

            PartSelectionManager._instance.crossHair.SetActive(false);

            //partGrid.ToggleTempBoundingBox(false);

            RestartText.text = "Restart";

            playerInput.SwitchCurrentActionMap("Player");
        }
        else if (playan)//because the level is reset with a LoadScene function, this else shouldn't really be called anymore. I left it in just in case.
        {
            //coreBlockPlayMode.SetActive(false);

            /* 
            List<Part> parts = FindObjectsOfType<Part>().ToList();
            foreach (Part part in parts)
            {
                if (part.useDirectionIndicator)
                    part.ToggleDirectionIndicator(true);

                if (part is MovementPart)
                {
                    part.GetComponent<MovementPart>().SwitchColliders();
                }
            }
            */

            //coreBlock.transform.position = coreBlockPlayMode.transform.position + new Vector3(0, 10, 0);
            //coreBlock.GetComponent<VehicleMovement>().enabled = false;
            //coreBlock.GetComponent<VehicleStats>().enabled = false;
            //Destroy(coreBlock.GetComponent<Rigidbody>());
            //coreBlock.transform.rotation = Quaternion.Euler(0, coreBlockPlayMode.transform.rotation.eulerAngles.y, 0);
            //mainCam.transform.SetPositionAndRotation(vehicleCam.transform.position, vehicleCam.transform.rotation);
            coreBlock.transform.position = coreBlock.transform.position + new Vector3(0, 10, 0);
            coreBlock.GetComponent<VehicleMovement>().enabled = false;
            coreBlock.GetComponent<ActivatePartActions>().enabled = false;
            Destroy(coreBlock.GetComponent<Rigidbody>());
            coreBlock.transform.rotation = Quaternion.Euler(0, coreBlock.transform.rotation.eulerAngles.y, 0);
            SetSelectedPart(selectedPart);
            coreBlock.transform.rotation = Quaternion.Euler(0, coreBlock.transform.rotation.eulerAngles.y, 0);
            vehicleCam.enabled = false;
            mainCam.gameObject.SetActive(true);

            coreBlock.SetActive(true);//reenable the original coreblock for editing
            Destroy(coreBlockPlayMode);//get rid of the old one

            PartSelectionManager._instance.ClosePartSelectionUI();
            ChangeActiveBuildState();

            if (statWindow == null)
            {
                statWindow = GameObject.Find("StatWindow");
                statWindow.SetActive(true);
            }
            else
                statWindow.SetActive(true);

            PartSelectionManager._instance.crossHair.SetActive(false);

            playan = false;

            RestartText.text = "Discard";

            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void PlacePart(InputAction.CallbackContext context)
    {
        if (!playan)
        {
            RaycastHit hit = RaycastMousePosition();
            if (hit.normal != Vector3.zero && hit.transform.TryGetComponent(out Part part) && !buildUIOpen)
            {
                if (context.action.name == "PlaceClick" && context.performed)
                {
                    PlaceSelectedPart(hit);
                }
                else if (context.action.name == "DeleteClick" && context.performed)
                {
                    DeleteSelectedPart(hit.transform);
                }
                else
                {
                    PreviewPart(hit);//assuming box colliders
                }
            }
            else if (previewedPart.activeSelf)
            {
                previewedPart.SetActive(false);
            }
        }
    }

    public void RotatePart(InputAction.CallbackContext context)
    {
        if (!playan && context.performed && !previewedPart.name.Contains("Wheel"))
        {
            partRotation.eulerAngles = Vector3Int.RoundToInt(partRotation.eulerAngles + new Vector3(0, 90, 0));
            PlacePart(context);
        }
    }

    public void RotatePartVertical(InputAction.CallbackContext context)
    {
        if (!playan && context.performed && !previewedPart.name.Contains("Wheel"))
        {
            if (vCount == 2)
            {
                partRotation.eulerAngles = Vector3Int.RoundToInt(partRotation.eulerAngles + new Vector3(-90, 0, 0));
            }
            else
            {
                partRotation.eulerAngles = Vector3Int.RoundToInt(partRotation.eulerAngles + new Vector3(90, 0, 0));
            }

            vCount++;
            if (vCount == 4)
            {
                vCount = 0;
            }
            PlacePart(context);
        }
    }

    void PlaceSelectedPart(RaycastHit hit)
    {
        Vector3Int pos = GetLocalPosition(hit);
        if (statWindow == null)
        {
            statWindow = GameObject.Find("StatWindow");
        }

        if (CheckPlacementValidity(hit))//check if the position the part would be placed in is in grid bounds
        {
            GameObject placedPart;

            if (selectedPart.GetComponent<Part>() is MovementPart)
                placedPart = Instantiate(selectedPart, coreBlock.transform.GetChild(0).transform);
            else
                placedPart = Instantiate(selectedPart, coreBlock.transform);

            statWindow.GetComponent<StatWindowUI>().UpdateStats(placedPart.GetComponent<Part>(), false);

            placedPart.transform.localPosition = pos;
            placedPart.transform.localRotation = partRotation;
            partGrid.AddPartToGrid(placedPart.GetComponent<Part>(), pos);

            if (placedPart.TryGetComponent(out Part part))
            {
                foreach (Part neighbour in partGrid.GetNeighbours(Vector3Int.RoundToInt(placedPart.transform.localPosition)))
                {
                    if (neighbour == null)
                    {
                        continue;
                    }
                    if (neighbour.transform == coreBlock.transform)
                        part.AttachPart(neighbour, pos);
                    else
                        part.AttachPart(neighbour, pos - neighbour.transform.localPosition);

                }
            }
            previewedPart.SetActive(false);
        }
    }

    private Vector3Int GetLocalPosition(RaycastHit hit)
    {
        Vector3Int pos = Vector3Int.RoundToInt(Quaternion.Inverse(coreBlock.transform.rotation) * hit.normal);
        if (hit.transform.parent != null)//the coreblock has no parent and, localPosition of an orphan gameObject would return it's world position. 
        {
            pos += Vector3Int.RoundToInt(hit.transform.localPosition);
        }
        return pos;
    }

    /// <summary>
    /// Check if the part has attachmentPoints available and is within the PartGrid bounds.
    /// </summary>
    /// <param name="pos"></param>
    private bool CheckPlacementValidity(RaycastHit hit)
    {
        Vector3Int hitNormal = Vector3Int.RoundToInt(hit.normal);
        if (partGrid.CheckIfInBounds(GetLocalPosition(hit)) && hit.transform.GetComponent<Part>().CheckIfAttachable(hitNormal)
            && previewedPart.transform.GetComponent<Part>().CheckIfAttachable(-hitNormal))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// detaches all parts from hit part and destroys the object.
    /// </summary>
    /// <param name="hit">the part that is hit</param>
    void DeleteSelectedPart(Transform partToDelete)
    {
        if (partToDelete != coreBlock.transform)
        {
            foreach (Part part in partToDelete.GetComponent<Part>().attachedParts)
            {
                if (part != null)
                    part.attachedParts[part.attachedParts.IndexOf(partToDelete.GetComponent<Part>())] = null;
            }

            partGrid.RemovePartFromGrid(Vector3Int.RoundToInt(partToDelete.localPosition));
            statWindow.GetComponent<StatWindowUI>().UpdateStats(partToDelete.gameObject.GetComponent<Part>(), true);
            Destroy(partToDelete.gameObject);
        }
    }

    public void DeleteAllParts()
    {
        foreach (Part part in coreBlock.GetComponentsInChildren<Part>())
        {
            DeleteSelectedPart(part.transform);
        }
        SetSelectedPart(selectedPart);
    }

    void PreviewPart(RaycastHit hit)
    {
        previewedPart.SetActive(true);

        Vector3Int newPos = GetLocalPosition(hit);

        previewedPart.transform.localPosition = newPos;
        previewedPart.transform.localRotation = partRotation;

        if (CheckPlacementValidity(hit))
        {
            previewedPart.GetComponent<PreviewPart>().SetMaterialColor(true);
        }
        else
        {
            previewedPart.GetComponent<PreviewPart>().SetMaterialColor(false);

        }
    }

    public void SetSelectedPart(GameObject slctPart)
    {
        selectedPart = slctPart;
        //instantiate the selected part for previewing
        Destroy(previewedPart);
        previewedPart = Instantiate(selectedPart, coreBlock.transform);
        previewedPart.AddComponent<PreviewPart>();
        if (previewedPart.TryGetComponent(out Collider col))
        {
            col.enabled = false;
        }
        previewedPart.SetActive(false);
    }


    public void ChangeActiveBuildState()
    {
        buildUIOpen = !buildUIOpen;
        if (buildUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /*
    public void CreateBoundingBox()
    {
        GameObject BoundingBoxPrefab = Resources.Load("BoundingBoxWithDirectionArrow") as GameObject;
        BoundingBox = Instantiate(BoundingBoxPrefab, coreBlock.transform);
        BoundingBox.transform.Translate(new Vector3(coreBlock.transform.position.x - (BoundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxW * 0.5f) - 0.1f,
                                                    coreBlock.transform.position.y - BoundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxH,
                                                    coreBlock.transform.position.z - (BoundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxL * 0.5f) - 1f));
    }
    */
    public void ResetPreviewRotation()
    {
        partRotation.eulerAngles = Vector3.zero;
    }

    public void RemovePreviewPart()
    {
        Destroy(previewedPart);
    }

    /// <summary>
    /// send a raycast from the mouse position 
    /// </summary>
    /// <returns>RaycastHit</returns>
    RaycastHit RaycastMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, 200);
        return hit;
    }
}
