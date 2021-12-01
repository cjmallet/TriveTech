using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    [Header("Bounding Box size")]
    public float length;
    public float width;
    public float height;


    private List<Transform> boundingBoxBorders = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            boundingBoxBorders.Add(child);
        }

        SetLength();
        SetWidth();
    }


    private void SetLength()
    {
        for (int i = 0; i < boundingBoxBorders.Count / 2; i++)
        {
            boundingBoxBorders[i].transform.localScale = new Vector3(boundingBoxBorders[i].transform.localScale.x,
                                                                     boundingBoxBorders[i].transform.localScale.y,
                                                                     length);

            boundingBoxBorders[i].transform.position = new Vector3(boundingBoxBorders[i].transform.position.x, 
                                                                   boundingBoxBorders[i].transform.position.x, 
                                                                   length / 2);
        }
    }
    
    private void SetWidth()
    {
        for (int i = 4; i < boundingBoxBorders.Count; i++)
        {
            boundingBoxBorders[i].transform.localScale = new Vector3(width,
                                                                     boundingBoxBorders[i].transform.localScale.y,
                                                                     boundingBoxBorders[i].transform.localScale.z);
        }
    }
    
    private void SetHeight()
    {
        for (int i = 0; i < boundingBoxBorders.Count / 2; i++)
        {
            if (i % 2 != 0)
            {
                boundingBoxBorders[i].transform.localScale = new Vector3(boundingBoxBorders[i].transform.localScale.x,
                                                                     boundingBoxBorders[i].transform.localScale.y,
                                                                     length);
            }
        }
    }
}
