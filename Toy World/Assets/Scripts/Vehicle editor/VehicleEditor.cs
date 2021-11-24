using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Quaternion partRotation;

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
        UpdateMouseInput();
    }

    void UpdateMouseInput()
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
            //NormalDirection(hit.normal);
        }
    }

    void NormalDirection(Vector3 normal)
    {
        switch (normal)
        {
            case Vector3 v when v.Equals(Vector3.up):
                Debug.Log("Up");
                break;
            case Vector3 v when v.Equals(Vector3.left):
                Debug.Log("Left");
                break;
            case Vector3 v when v.Equals(Vector3.back):
                Debug.Log("Back");
                break;
            case Vector3 v when v.Equals(Vector3.forward):
                Debug.Log("Forward");
                break;
            case Vector3 v when v.Equals(Vector3.down):
                Debug.Log("Down");
                break;
            case Vector3 v when v.Equals(Vector3.right):
                Debug.Log("Right");
                break;
        }

    }

    void PlaceSelectedPart(RaycastHit hit)
    {
        GameObject placedPart = Instantiate(
            selectedPart, hit.transform.localPosition + hit.normal + coreBlock.transform.position, partRotation,coreBlock.transform);
        
        if(placedPart.TryGetComponent(out Part part))
        {
            part.AttachPart(hit.transform.GetComponent<Part>(), hit.normal);
        }

        /* X instantiate selectedPart
         * X call the attachPart on the raycast hit and the selected part, but with flipped normal(maybe automate this in the part class?)
         * X figure out where to run the partToPlace code
         * make a 3D array and check for neighbours to call the attach fuction for.
         */
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
     * show preview part, if it doesn't fit, show it with a red material
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
