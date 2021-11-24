using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private GameObject coreBlock;
    [SerializeField] private GameObject selectedPart;

    private GameObject previewedPart;
    private Vector3 prevMousePos;
    public Quaternion partRotation;

    void Awake()
    {
        //set the static instance
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    void Start()
    {
        SetSelectedPart(selectedPart);
        coreBlock = GameObject.Find("CoreBlock");
    }

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        RaycastHit hit = RaycastMousePosition();
        if (hit.normal != Vector3.zero && hit.transform.TryGetComponent(out Part part)) //(hit.transform.GetComponent<TempPart>() != null)
        {
            Debug.Log("part was hit");
            PreviewPart(hit.transform.position + hit.normal);//assuming box colliders
            Debug.Log($"hit pos: {hit.transform.position} hit normal: {hit.normal}");
        }
        else if (previewedPart.activeSelf)
        {
            previewedPart.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlaceSelectedPart(hit);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(partRotation.eulerAngles.y == 270)
            {
                partRotation.eulerAngles = Vector3.zero;
            }
            else
            {
                partRotation.eulerAngles = (partRotation.eulerAngles + new Vector3(0, 90, 0));
            }
        }
    }

    void PlaceSelectedPart(RaycastHit hit)
    {
        GameObject placedPart = Instantiate(
            selectedPart, Vector3Int.RoundToInt(hit.transform.localPosition + hit.normal + coreBlock.transform.position), partRotation,coreBlock.transform);
        
        if(placedPart.TryGetComponent(out Part part))
        {
            part.AttachPart(hit.transform.GetComponent<Part>(), hit.normal);
        }
       
    }

    void PreviewPart(Vector3 pos)//todo:
    {
        previewedPart.SetActive(true);
        previewedPart.transform.position = pos;
    }

    public void SetSelectedPart(GameObject slctPart)
    {
        Destroy(previewedPart);
        selectedPart = slctPart;
        previewedPart = Instantiate(selectedPart);
        if (previewedPart.TryGetComponent(out Collider col))
        {
            col.enabled = false;
        }
        previewedPart.SetActive(false);
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
        //Debug.Log("Hit normal vector = "+hit.normal);//remove debug later
        return hit;
    }

}
