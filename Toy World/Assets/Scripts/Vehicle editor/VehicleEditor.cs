using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleEditor : MonoBehaviour
{
    
    [SerializeField] private TempPart coreBlock;
    [SerializeField] private TempPart selectedPart;//could remove serializable tags later on.
    
    void Start()
    {
        if(coreBlock = null)
        {
            coreBlock = GameObject.Find("CoreBlock").GetComponent<TempPart>();
        }
    }

    void Update()
    {
        
    }

    /* instantiate 3D array in which every part has it's coordinates 
     * hype
     * check if raycast hits part
     *check if part has available slot there. 
     * show preview part, if it doesn't fit, show it with a red material
     * if it fits. Click to place the part and tell
    */



    RaycastHit RaycastMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 80))
        {
            Debug.Log(hit.point);//remove debug later
            return hit;
        }
        return hit;
    }
    
}
