using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// The vehicle editor handles the creation of the vehicle. It places parts on the coreblock and registers the connections of the parts.
/// On play, it creates a copy of the coreblock and sets it up for play mode. This means things like switching colliders on and off and setting/resetting certain values.
/// By Ruben de Graaf, mostly.
/// </summary>
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
    private List<Part> movementParts = new List<Part>();

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

        statWindow.GetComponent<StatWindowUI>().allParts = partGrid.ReturnAllParts();

        if (movementParts.Count > 0)
            movementParts.Clear();
        movementParts = partGrid.ReturnAllParts().Where(x => x.TryGetComponent(out MovementPart part)).ToList();

        statWindow.GetComponent<StatWindowUI>().SetupAllParts(movementParts.Count);
    }

    /// <summary>
    /// Creates a copy of the coreBlock from the DontDestroyOnLoad reference and prepares it for play mode. 
    /// Sets up the rigid body, sets up the movement parts, correctly sets the part values for the UI and 
    /// recreates the partgrid on the new core block, since array references don't copy properly during instantiate.
    /// </summary>
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
                if (vehiclePart.transform.localPosition.z > 0)
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
        //vehicleCam = coreBlockPlayMode.GetComponentInChildren<Camera>();
        vehicleCam.gameObject.SetActive(true);

        coreBlockPlayMode.SetActive(true);
    }

    /// <summary>
    /// When input is given, call PlaceSelectedPart() or DeleteSelectedPart() on the raycasted part. 
    /// Uses the previewedPart object to indicate if the placement is valid and what the rotation is like.
    /// </summary>
    /// <param name="context">Input context</param>
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

    /// <summary>
    /// Rotates the currently selected part over the Y axis.
    /// </summary>
    /// <param name="context">Input context</param>
    public void RotatePartYaxis(InputAction.CallbackContext context)
    {
        if (context.performed && !previewedPart.name.Contains("Wheel") && !previewedPart.name.Contains("Launcher"))
        {
            previewedPart.transform.Rotate(0, 90, 0);
            partRotation = previewedPart.transform.rotation; // You know what, fuck this.

            PlacePart(context);
        }
    }

    /// <summary>
    /// Rotates the currently selected part over X axis.
    /// </summary>
    /// <param name="context">Input context</param>
    public void RotatePartXaxis(InputAction.CallbackContext context)
    {
        if (context.performed && !previewedPart.name.Contains("Wheel") && !previewedPart.name.Contains("Launcher"))
        {
            previewedPart.transform.Rotate(90, 0, 0);
            partRotation = previewedPart.transform.rotation; // You know what, fuck this.

            PlacePart(context);
        }
    }
    /// <summary>
    /// Rotates the currently selected part over Z axis.
    /// </summary>
    /// <param name="context">Input context</param>
    public void RotatePartZaxis(InputAction.CallbackContext context)
    {
        if (context.performed && !previewedPart.name.Contains("Wheel") && !previewedPart.name.Contains("Launcher"))
        {
            previewedPart.transform.Rotate(0, 0, 90);
            partRotation = previewedPart.transform.rotation; // You know what, fuck this.

            PlacePart(context);
        }
    }

    /// <summary>
    /// Used to place the currently selected part on another part, registers the connections between the parts and adds it to the PartGrid component on the coreBlock. 
    /// Uses CheckPlacementValidity() to check if placement is possible.
    /// This also updates the stats UI window.
    /// </summary>
    /// <param name="hit"> raycast hit to check which part and which side on said part to attach to</param>
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
            {
                placedPart = Instantiate(selectedPart, coreBlock.transform.GetChild(0).transform);
                movementParts.Add(placedPart.GetComponent<Part>());
            }                
            else
                placedPart = Instantiate(selectedPart, coreBlock.transform);

            statWindow.GetComponent<StatWindowUI>().UpdateStats(placedPart.GetComponent<Part>(), false, movementParts.Count);

            placedPart.transform.localPosition = pos;
            placedPart.transform.localRotation = partRotation;
            partGrid.AddPartToGrid(placedPart.GetComponent<Part>(), pos);

            if (placedPart.TryGetComponent(out Part part))
            {
                foreach (Part neighbour in partGrid.GetNeighbours(Vector3Int.RoundToInt(placedPart.transform.localPosition)))
                {
                    AudioManager.Instance.Play(AudioManager.clips.PlacePart, Camera.main.GetComponent<AudioSource>());

                    if (neighbour == null)
                    {
                        continue;
                    }
                    if (neighbour.transform == coreBlock.transform)
                    {
                        
                        part.AttachPart(neighbour, pos);
                    }                        
                    else
                        part.AttachPart(neighbour, pos - neighbour.transform.localPosition);

                }
            }
            previewedPart.SetActive(false);
        }
    }

    /// <summary>
    /// A raycast hit gives a world position. This is used to translate that hit to a perfectly rounded position relative to the coreblock.
    /// </summary>
    /// <param name="hit"></param>
    /// <returns>Rounded Vector3Int to make sure no accidental float roundoffs are returned. These will be used as indices in the PartGrid.</returns>
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
    /// Check if the part has attachmentPoints available, isn't already connected and is within the PartGrid bounds.
    /// </summary>
    /// <param name="pos"></param>
    private bool CheckPlacementValidity(RaycastHit hit)
    {
        Vector3Int hitNormal = Vector3Int.RoundToInt(hit.normal);

        if (previewedPart.TryGetComponent(out MovementPart part))
        {
            if (movementParts.Count < 20 && partGrid.CheckIfInBounds(GetLocalPosition(hit)) && hit.transform.GetComponent<Part>().CheckIfAttachable(hitNormal)
                && previewedPart.transform.GetComponent<Part>().CheckIfAttachable(-hitNormal))
            {
                return true;
            }
            else
            {
                return false;
            }    
        }
        else
        {            
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

            if (partToDelete.TryGetComponent(out MovementPart movePart))
                movementParts.Remove(movePart);

            partGrid.RemovePartFromGrid(Vector3Int.RoundToInt(partToDelete.localPosition));
            statWindow.GetComponent<StatWindowUI>().UpdateStats(partToDelete.gameObject.GetComponent<Part>(), true, movementParts.Count);
            AudioManager.Instance.Play(AudioManager.clips.RemovePart, Camera.main.GetComponent<AudioSource>());
            Destroy(partToDelete.gameObject);
        }
    }

    /// <summary>
    /// Deletes and unregisters all parts currently attached to the coreBlock.
    /// </summary>
    public void DeleteAllParts()
    {
        foreach (Part part in coreBlock.GetComponentsInChildren<Part>())
        {
            DeleteSelectedPart(part.transform);            
        }
        movementParts.Clear();

        SetSelectedPart(selectedPart);
    }

    /// <summary>
    /// Enables the previewedPart, places it at the hit location and makes it green or red, based on the validity of the potential placement.
    /// </summary>
    /// <param name="hit">Part that is raycasted based on mouse pos</param>
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

    /// <summary>
    /// Called from the Build UI. Sets the currently selected part and recreates the previewedPart to represent it.
    /// </summary>
    /// <param name="slctPart">Newly selected part to place and preview</param>
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

    /// <summary>
    /// Resets the current set part rotation. Used when parts that shouldn't be rotated are selected.
    /// </summary>
    public void ResetPreviewRotation()
    {
        partRotation.eulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Deletes the previewedPart. 
    /// </summary>
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
