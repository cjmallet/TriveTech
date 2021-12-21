using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

/* TODO:
 * make a 3D array and check for neighbours to call the attach function for.
 * if preview part doesn't fit, show it with a red material
 * actually for the part class, but make it so you can have parts that are bigger than 1x1x1
 */

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
    [SerializeField] private GameObject selectedPart;
    private PartGrid partGrid;

    private GameObject BoundingBoxPrefab;
    private GameObject BoundingBox;

    private GameObject previewedPart;
    private bool playan, buildUIOpen = true;
    private Camera mainCam;

    private int vCount;

    public Camera vehicleCam;

    [SerializeField]
    private PlayerInput playerInput;


    void Awake()
    {
        //set the static instance
        if (instance == null) { instance = this; }
        else { Destroy(this); }
        coreBlock = GameObject.Find("CoreBlock");
    }

    void Start()
    {
        partGrid = coreBlock.GetComponent<PartGrid>();
        playerInput.SwitchCurrentActionMap("UI");
        SetSelectedPart(selectedPart);
        mainCam = Camera.main;
        if (vehicleCam == null)
        {
            vehicleCam = coreBlock.GetComponentInChildren<Camera>();
        }

        CreateBoundingBox();
    }

    public void Play()
    {
        if (!playan)
        {
            // Clear list of parts if it still has parts
            if (coreBlock.GetComponent<VehicleMovement>().wheelInfos.Count != 0)
                coreBlock.GetComponent<VehicleMovement>().wheelInfos.Clear();

            coreBlock.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            coreBlock.GetComponent<Rigidbody>().mass = 1000f;
            coreBlock.GetComponent<Rigidbody>().drag = 0.5f;

            // De manier van het vullen van deze list moet uiteraard veranderd worden wanneer het Grid (3D vector) systeem er is.
            List<Part> parts = coreBlock.GetComponent<PartGrid>().ReturnAllParts();
            coreBlock.GetComponent<VehicleMovement>().allParts = parts;
            coreBlock.GetComponent<VehicleStats>().allParts = parts;

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
                    coreBlock.GetComponent<VehicleMovement>().AddWheel((MovementPart)vehiclePart);
                    vehiclePart.GetComponent<MovementPart>().SwitchColliders();
                }

                // Remove direction indication
                if (vehiclePart.useDirectionIndicator)
                    vehiclePart.ToggleDirectionIndicator(false);
            }

            coreBlock.GetComponent<VehicleMovement>().enabled = true;
            coreBlock.GetComponent<VehicleStats>().enabled = true;
            mainCam.gameObject.SetActive(false);
            vehicleCam.enabled = true;
            playan = true;
            previewedPart.SetActive(false);
            BoundingBox.SetActive(false);

            if (buildUIOpen)
            {
                PartSelectionManager._instance.ClosePartSelectionUI();
                ChangeActiveBuildState();
            }
            PartSelectionManager._instance.crossHair.SetActive(false);

            partGrid.ToggleTempBoundingBox(false);

            playerInput.SwitchCurrentActionMap("Player");
        }
        else if (playan)
        {
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

            coreBlock.transform.position = coreBlock.transform.position + new Vector3(0, 10, 0);
            coreBlock.GetComponent<VehicleMovement>().enabled = false;
            coreBlock.GetComponent<VehicleStats>().enabled = false;
            Destroy(coreBlock.GetComponent<Rigidbody>());
            coreBlock.transform.rotation = Quaternion.Euler(0, coreBlock.transform.rotation.eulerAngles.y, 0);
            vehicleCam.enabled = false;
            mainCam.transform.SetPositionAndRotation(vehicleCam.transform.position, vehicleCam.transform.rotation);
            mainCam.gameObject.SetActive(true);
            BoundingBox.SetActive(true);
            partGrid.ToggleTempBoundingBox(true);

            PartSelectionManager._instance.ClosePartSelectionUI();
            ChangeActiveBuildState();
            PartSelectionManager._instance.crossHair.SetActive(false);

            playan = false;

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
                    DeleteSelectedPart(hit);
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
        if (!playan && context.performed)
        {
            partRotation.eulerAngles = Vector3Int.RoundToInt(partRotation.eulerAngles + new Vector3(0, 90, 0));
            PlacePart(context);
        }
    }

    public void RotatePartVertical(InputAction.CallbackContext context)
    {
        if (!playan && context.performed)
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
            if (vCount==4)
            {
                vCount = 0;
            }
            PlacePart(context);
        }
    }

    void PlaceSelectedPart(RaycastHit hit)
    {
        Vector3Int pos= GetLocalPosition(hit);

        if (CheckCorrectPlacement(hit))//check if the position the part would be placed in is in grid bounds
        {
            GameObject placedPart;

            if (selectedPart.GetComponent<Part>() is MovementPart)
                placedPart = Instantiate(selectedPart, coreBlock.transform.GetChild(0).transform);
            else
                placedPart = Instantiate(selectedPart, coreBlock.transform);

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
    /// Check if the part has attachmentPoints available and is in bounds off the boundingbox.
    /// </summary>
    /// <param name="pos"></param>
    private bool CheckCorrectPlacement(RaycastHit hit)
    {
        Vector3Int localposition = GetLocalPosition(hit);
        Vector3Int hitNormal = Vector3Int.RoundToInt(hit.normal);
        if (partGrid.CheckIfInBounds(localposition) &&hit.transform.GetComponent<Part>().CheckCorrectSide(hitNormal))
        {
            return true;
        }
        else
        {
            Debug.LogError("Not correct placement");
            return false;
        }
    }

    /// <summary>
    /// detaches all parts from hit part and destroys the object.
    /// </summary>
    /// <param name="hit">the part that is hit</param>
    void DeleteSelectedPart(RaycastHit hit)
    {
        if (hit.transform != coreBlock.transform)
        {
            foreach (Part part in hit.transform.GetComponent<Part>().attachedParts)
            {
                if (part != null)
                {
                    part.attachedParts[part.attachedParts.IndexOf(hit.transform.GetComponent<Part>())] = null;
                }
            }
            partGrid.RemovePartFromGrid(Vector3Int.RoundToInt(hit.transform.localPosition));
            Destroy(hit.transform.gameObject);
        }
    }

    void PreviewPart(RaycastHit hit)
    {
        previewedPart.SetActive(true);

        Vector3Int newPos = GetLocalPosition(hit);

        previewedPart.transform.localPosition = newPos;
        previewedPart.transform.localRotation = partRotation;

        if (CheckCorrectPlacement(hit))
        {
            previewedPart.transform.localScale = Vector3.one;
        }
        else
        {
            previewedPart.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
           
        }
    }

    public void SetSelectedPart(GameObject slctPart)
    {
        selectedPart = slctPart;
        //instantiate the selected part for previewing
        Destroy(previewedPart);
        previewedPart = Instantiate(selectedPart, coreBlock.transform);
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

    private void CreateBoundingBox()
    {
        BoundingBoxPrefab = Resources.Load("BoundingBoxWithDirectionArrow") as GameObject;
        BoundingBox = Instantiate(BoundingBoxPrefab, coreBlock.transform);
        BoundingBox.transform.Translate(new Vector3(coreBlock.transform.position.x - BoundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxW * 0.5f,
                                                    coreBlock.transform.position.y - BoundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxH * 0.75f,
                                                    coreBlock.transform.position.z - BoundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxL * 0.5f));
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
