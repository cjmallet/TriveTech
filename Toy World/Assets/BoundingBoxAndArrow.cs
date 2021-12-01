using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxAndArrow : MonoBehaviour
{
    public float boxL { get; set; }
    public float boxW { get; set; }
    public float boxH { get; set; }

    public float arrowL { get; set; }
    public float arrowW { get; set; }


    public BoundingBoxAndArrow()
    {
        this.boxW = 20f;
        this.boxL = 20f;
        this.boxH = 10f;
    }
}
