using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleEditor : MonoBehaviour
{
    
    [SerializeField] private TempPart coreBlock;
    [SerializeField] private GameObject selectedPart;//

    private Vector3 prevMousePos;
    
    void Start()
    {
        SetSelectedPart(selectedPart);
        if (coreBlock = null)
        {
            coreBlock = GameObject.Find("CoreBlock").GetComponent<TempPart>();
        }
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
            Func();

    }

    void Func()
    {
        if (Input.mousePosition != prevMousePos)
        {
            RaycastHit hit = RaycastMousePosition();
            if (hit.transform.TryGetComponent(out TempPart part)) //(hit.transform.GetComponent<TempPart>() != null)
            {
                PreviewPart(hit.transform.position + hit.normal);//assuming box colliders
            }
            else if (selectedPart.activeSelf)
            {
                selectedPart.SetActive(false);
            }

            prevMousePos = Input.mousePosition;
        }
    }

    void PlacePart(RaycastHit hit)
    {

    }

    void PreviewPart(Vector3 pos)//todo:
    {
        selectedPart.SetActive(true);
        selectedPart.transform.position = pos;
    }

    public void SetSelectedPart(GameObject selectedPart)
    {
        Destroy(this.selectedPart);
        this.selectedPart = selectedPart;
        Instantiate(this.selectedPart);
        if(selectedPart.TryGetComponent(out Collider col)){
            col.enabled = false;
        }
        selectedPart.SetActive(false);
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

        Physics.Raycast(ray, out hit, 100);
        Debug.Log("Hit location vector = "+hit.point);//remove debug later
        return hit;
    }
    
}
