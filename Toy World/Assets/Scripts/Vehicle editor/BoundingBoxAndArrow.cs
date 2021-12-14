using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxAndArrow : MonoBehaviour
{
    public float boxL;
    public float boxW;
    public float boxH;

    public float arrowW;
    public float arrowL;

    public float MyLength { get; set; }
    public float MyWidth { get; set; }
    public float MyHeight { get; set; }
    public float MyArrowWidth { get; set; }
    public float MyArrowLength { get; set; }

    private void Start()
    {
        MyLength = boxL;
        MyWidth = boxW;
        MyHeight = boxH;
        MyArrowWidth = arrowW;
        MyArrowLength = arrowL;

        transform.GetChild(0).GetComponent<BoundingBox>().MyHeight = MyHeight;
        transform.GetChild(0).GetComponent<BoundingBox>().MyWidth = MyWidth;
        transform.GetChild(0).GetComponent<BoundingBox>().MyLength = MyLength;

        transform.GetChild(0).transform.GetChild(0).GetComponent<DirectionArrowBoundingBox>().MyArrowWidth = MyArrowWidth;
        transform.GetChild(0).transform.GetChild(0).GetComponent<DirectionArrowBoundingBox>().MyArrowLength = MyArrowLength;
        transform.GetChild(0).transform.GetChild(0).GetComponent<DirectionArrowBoundingBox>().MyWidth = MyWidth;
        transform.GetChild(0).transform.GetChild(0).GetComponent<DirectionArrowBoundingBox>().MyLength = MyLength;
    }
}
