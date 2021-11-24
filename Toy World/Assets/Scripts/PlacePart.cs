using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePart : MonoBehaviour
{
    private float raycastDistance = 200f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                Debug.Log("Clicked");
            }
        }
    }
}
