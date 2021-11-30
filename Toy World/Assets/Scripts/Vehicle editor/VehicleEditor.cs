using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

/* TODO:
 * make a 3D array and check for neighbours to call the attach function for.
 * ROUND OFF EVERY NORMAL, ROTATION AND POSITION USED to fit in a grid
 * Delete part function tool
 * foolproof the editor for whack non-90 degrees rotations
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

    private GameObject previewedPart;
    private Vector3 prevMousePos;
    private bool playan, buildUIOpen = true;


    void Awake()
    {
        //set the static instance
        if (instance == null) { instance = this; }
        else { Destroy(this); }
        coreBlock = GameObject.Find("CoreBlock");
    }

    void Start()
    {
        SetSelectedPart(selectedPart);
    }

    void Update()
    {
        
    }

    public void Play(InputAction.CallbackContext context)
    {
        if (context.performed && !playan)
        {
            coreBlock.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            coreBlock.AddComponent<VehicleMovement>();
            coreBlock.GetComponent<VehicleMovement>().movementParts = FindObjectsOfType<MovementPart>().ToList();
            Camera.main.enabled = false;
            coreBlock.GetComponentInChildren<Camera>().enabled = true;
            playan = true;
            Destroy(previewedPart);
        }
    }

    public void PlacePart(InputAction.CallbackContext context)
    {
        if (!playan)
        {
            RaycastHit hit = RaycastMousePosition();
            if (hit.normal != Vector3.zero && hit.transform.TryGetComponent(out Part part) && !buildUIOpen)
            {
                if (context.action.name == "LeftClick" && context.performed)
                {
                    PlaceSelectedPart(hit);
                    previewedPart.SetActive(false);
                }
                else if (context.action.name == "RightClick" && context.performed)
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

    void PlaceSelectedPart(RaycastHit hit)
    {
        GameObject placedPart = Instantiate(selectedPart, coreBlock.transform);

        if(hit.transform == coreBlock.transform)
        {
            placedPart.transform.localPosition = Vector3Int.RoundToInt(Quaternion.Inverse(coreBlock.transform.rotation) * hit.normal);
        }
        else
        {
            placedPart.transform.localPosition = Vector3Int.RoundToInt(Quaternion.Inverse(coreBlock.transform.rotation) * hit.normal + hit.transform.localPosition);
        }
        placedPart.transform.localRotation = partRotation;

        if (placedPart.TryGetComponent(out Part part))
        {
            part.AttachPart(hit.transform.GetComponent<Part>(), hit.normal);
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
            Destroy(hit.transform.gameObject);
        }
    }

    void PreviewPart(RaycastHit hit)
    {
        previewedPart.SetActive(true);

        if (hit.transform == coreBlock.transform)
        {
            previewedPart.transform.localPosition = Vector3Int.RoundToInt(Quaternion.Inverse(coreBlock.transform.rotation) * hit.normal);
        }
        else
        {
            previewedPart.transform.localPosition = Vector3Int.RoundToInt(Quaternion.Inverse(coreBlock.transform.rotation) * hit.normal + hit.transform.localPosition);
        }
        previewedPart.transform.localRotation = partRotation;
    }

    public void SetSelectedPart(GameObject slctPart)
    {
        Destroy(previewedPart);
        selectedPart = slctPart;
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
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    /* instantiate 3D array in which every part has it's coordinates 
     * hype
     * check if raycast hits part
     * check if part has available slot there. 
     * 
     * if it fits. Click to place the part and tell
    */


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
