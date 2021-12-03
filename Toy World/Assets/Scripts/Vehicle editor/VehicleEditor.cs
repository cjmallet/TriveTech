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

    private GameObject BoundingBoxPrefab;
    private GameObject BoundingBox;

    private GameObject previewedPart;
    private Vector3 prevMousePos;
    private bool playan, buildUIOpen = true;
    private Camera mainCam;
    public Camera vehicleCam;


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
        mainCam = Camera.main;
        if (vehicleCam == null)
        {
            vehicleCam = coreBlock.GetComponentInChildren<Camera>();
        }

        CreateBoundingBox();
    }

    void Update()
    {
        
    }

    public void Play(InputAction.CallbackContext context)
    {
        if (context.performed && !playan)
        {
            // Clear list of parts if it still has parts
            if (coreBlock.GetComponent<VehicleMovement>().movementParts.Count != 0)
                coreBlock.GetComponent<VehicleMovement>().movementParts.Clear();

            coreBlock.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            coreBlock.GetComponent<Rigidbody>().drag = 1;

            // De manier van het vullen van deze list moet uiteraard veranderd worden wanneer het Grid (3D vector) systeem er is.
            List<Part> parts = FindObjectsOfType<Part>().ToList();
            coreBlock.GetComponent<VehicleMovement>().allParts = parts;

            // Remove direction indication
            foreach (Part vehiclePart in parts)
            {
                // Fill list with movement parts for movement script
                if (vehiclePart is MovementPart)
                    coreBlock.GetComponent<VehicleMovement>().movementParts.Add((MovementPart)vehiclePart);

                // Remove direction indication
                if (vehiclePart.useDirectionIndicator)
                    vehiclePart.ToggleDirectionIndicator(false);
            }

            coreBlock.GetComponent<VehicleMovement>().enabled = true;
            mainCam.gameObject.SetActive(false);
            vehicleCam.enabled = true;
            playan = true;
            previewedPart.SetActive(false);
            BoundingBox.SetActive(false);
        }
        else if (context.performed && playan)
        {
            List<Part> parts = FindObjectsOfType<Part>().ToList();
            foreach (Part part in parts)
            {
                if (part.useDirectionIndicator)
                    part.ToggleDirectionIndicator(true);
            }

            coreBlock.transform.position = coreBlock.transform.position + new Vector3(0, 10, 0);            
            coreBlock.GetComponent<VehicleMovement>().enabled = false;
            Destroy(coreBlock.GetComponent<Rigidbody>());
            coreBlock.transform.rotation = Quaternion.Euler(0, coreBlock.transform.rotation.eulerAngles.y, 0);
            mainCam.transform.SetPositionAndRotation(vehicleCam.transform.position, vehicleCam.transform.rotation);
            mainCam.gameObject.SetActive(true);
            BoundingBox.SetActive(true);

            playan = false;
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
        if (buildUIOpen || !Camera.main.GetComponent<FPSCameraControllers>().enabled) // If FPS camera controller is disabled cursor is always unlocked
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
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
