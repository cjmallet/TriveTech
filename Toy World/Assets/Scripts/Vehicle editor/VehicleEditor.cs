using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;

public class VehicleEditor : MonoBehaviour
{
    public Quaternion partRotation;
    [SerializeField] private GameObject coreBlock;
    public GameObject coreBlockPlayMode;
    [SerializeField] private GameObject selectedPart;
    private PartGrid partGrid;

    private GameObject previewedPart;

    private int vCount;

    public Camera vehicleCam;

    private GameObject statWindow;

    private List<Part> parts = new List<Part>();

    void Start()
    {
        coreBlock = DDOL.Instance.P1Coreblock;
        coreBlock.SetActive(true);
        partGrid = coreBlock.GetComponent<PartGrid>();
        statWindow = GameObject.Find("StatWindow");        
        SetSelectedPart(selectedPart);
        
        if (vehicleCam == null)
        {
            vehicleCam = coreBlock.GetComponentInChildren<Camera>();
        }

        statWindow.GetComponent<StatWindowUI>().allParts = partGrid.allParts;
        statWindow.GetComponent<StatWindowUI>().SetupAllParts();
    }

    public void PrepareVehicle()
    {
        DestroyImmediate(previewedPart);

        if (statWindow == null)
        {
            statWindow = GameObject.Find("StatWindow");
            statWindow.SetActive(false);
        }
        else
            statWindow.SetActive(false);

        coreBlock.SetActive(false); //OG coreblock is disabled until back in build mode

        //make a copy of the coreblock for playmode
        coreBlockPlayMode = Instantiate(coreBlock, coreBlock.transform.position, coreBlock.transform.rotation);

        // Clear list of parts if it still has parts
        if (coreBlockPlayMode.GetComponent<VehicleMovement>().wheelInfos.Count != 0)
            coreBlockPlayMode.GetComponent<VehicleMovement>().wheelInfos.Clear();

        if (parts.Count != 0)
            parts.Clear();

        coreBlockPlayMode.GetComponent<PartGrid>().RemakePartGrid();
        coreBlockPlayMode.GetComponent<PartGrid>().ToggleBoundingBox(false);        

        parts = coreBlockPlayMode.GetComponent<PartGrid>().ReturnAllParts();
        coreBlockPlayMode.GetComponent<VehicleMovement>().allParts = parts;
        coreBlockPlayMode.GetComponent<ActivatePartActions>().CategorizePartsInList(parts);

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

        coreBlockPlayMode.GetComponent<VehicleMovement>().enabled = true;
        coreBlockPlayMode.GetComponent<ActivatePartActions>().enabled = true;
        coreBlockPlayMode.GetComponent<PartGrid>().CheckConnection();

        //grab vehiclecam again, because we have a new coreblock instance for playmode
        vehicleCam = coreBlockPlayMode.GetComponentInChildren<Camera>();
        vehicleCam.enabled = true;

        coreBlockPlayMode.SetActive(true);
    }

    public void PlacePart(InputAction.CallbackContext context)
    {
        RaycastHit hit = RaycastMousePosition();
        if (hit.normal != Vector3.zero && hit.transform.TryGetComponent(out Part part) && !GameManager.Instance.partSelectionManager.buildUIOpen)
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
        else if (previewedPart != null && previewedPart.activeSelf)
        {
            previewedPart.SetActive(false);
        }
    }

    public void RotatePart(InputAction.CallbackContext context)
    {
        if (context.performed && !previewedPart.name.Contains("Wheel"))
        {
            partRotation.eulerAngles = Vector3Int.RoundToInt(partRotation.eulerAngles + new Vector3(0, 90, 0));
            PlacePart(context);
        }
    }

    public void RotatePartVertical(InputAction.CallbackContext context)
    {
        if (context.performed && !previewedPart.name.Contains("Wheel"))
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

    public void ResetPreviewRotation()
    {
        partRotation.eulerAngles = Vector3.zero;
    }

    /// <summary>
    /// send a raycast from the mouse position 
    /// </summary>
    /// <returns>RaycastHit</returns>
    RaycastHit RaycastMousePosition()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit, 200);
            return hit;
        }
        return new RaycastHit(); // fake
    }
}
