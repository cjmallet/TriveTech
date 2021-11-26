using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// deprecated
/// </summary>
public class PartToBePlaced : Part
{
    /*
    public void PlacePart(RaycastHit hit)
    {

        if (hit.normal.x == 1)
        {
            Debug.Log("Right");
            // Find a way to know what part to add and use it 
            // Tag from prefab item perhaps?
            this.gameObject.AddComponent(typeof(TestPart));
            GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Right);
            Destroy(this);
        }

        if (hit.normal.x == -1)
        {
            Debug.Log("Left");
            this.gameObject.AddComponent(typeof(TestPart));
            GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Left);
            Destroy(this);
        }

        if (hit.normal.y == 1)
        {
            Debug.Log("Top");
            this.gameObject.AddComponent(typeof(TestPart));
            GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Top);
            Destroy(this);
        }

        if (hit.normal.y == -1)
        {
            Debug.Log("Bottom");
            this.gameObject.AddComponent(typeof(TestPart));
            GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Bottom);
            Destroy(this);
        }

        if (hit.normal.z == 1)
        {
            Debug.Log("Back");
            this.gameObject.AddComponent(typeof(TestPart));
            GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Back);
            Destroy(this);
        }

        if (hit.normal.z == -1)
        {
            Debug.Log("Front");
            this.gameObject.AddComponent(typeof(TestPart));
            GetComponent<TestPart>().AttachPart(hit.transform.GetComponent<TestPart>(), Part.Orientation.Front);
            Destroy(this);
        }


    }
    */
}
