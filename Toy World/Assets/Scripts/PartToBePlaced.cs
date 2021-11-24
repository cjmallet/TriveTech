using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartToBePlaced : Part
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
                if (hit.normal.x == 1 && hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Right] == null)
                {
                    Debug.Log("Right");
                    // Find a way to know what part to add and use it 
                    // Tag from prefab item perhaps?
                    this.gameObject.AddComponent(typeof(TestPart));
                    GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Right);
                    Destroy(this);
                }
                else if (hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Right] != null)
                    Debug.LogWarning("Side already connected!");

                if (hit.normal.x == -1 && hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Left] == null)
                {
                    Debug.Log("Left");
                    this.gameObject.AddComponent(typeof(TestPart));
                    GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Left);
                    Destroy(this);
                }
                else if (hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Left] != null)
                    Debug.LogWarning("Side already connected!");

                if (hit.normal.y == 1 && hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Top] == null)
                {
                    Debug.Log("Top");
                    this.gameObject.AddComponent(typeof(TestPart));
                    GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Top);
                    Destroy(this);
                }
                else if (hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Top] != null)
                    Debug.LogWarning("Side already connected!");

                if (hit.normal.y == -1 && hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Bottom] == null)
                {
                    Debug.Log("Bottom");
                    this.gameObject.AddComponent(typeof(TestPart));
                    GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Bottom);
                    Destroy(this);
                }
                else if (hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Bottom] != null)
                    Debug.LogWarning("Side already connected!");

                if (hit.normal.z == 1 && hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Back] == null)
                {
                    Debug.Log("Back");
                    this.gameObject.AddComponent(typeof(TestPart));
                    GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Back);
                    Destroy(this);
                }
                else if (hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Back] != null)
                    Debug.LogWarning("Side already connected!");

                if (hit.normal.z == -1 && hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Front] == null)
                {
                    Debug.Log("Front");
                    this.gameObject.AddComponent(typeof(TestPart));
                    GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Front);
                    Destroy(this);
                }
                else if (hit.transform.GetComponent<Part>().attachedParts[(int)Part.Orientation.Front] != null)
                    Debug.LogWarning("Side already connected!");
            }
        }
    }
}
