using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : BoundingBoxAndArrow
{
    void Start()
    {
        CreateBoundingBox();
    }

    private void CreateBoundingBox()
    {
        Vector3[] vertices = {

            // Create bounding box
            new Vector3(0, boxH, 0),
            new Vector3(0, 0, 0),
            new Vector3(boxW, boxH, 0),
            new Vector3(boxW, 0, 0),
            
            new Vector3(0, 0, boxL),
            new Vector3(boxW, 0, boxL),
            new Vector3(0, boxH, boxL),
            new Vector3(boxW, boxH, boxL),

            new Vector3(0, boxH, 0),
            new Vector3(boxW, boxH, 0),

            new Vector3(0, boxH, 0),
            new Vector3(0, boxH, boxL),

            new Vector3(boxW, boxH, 0),
            new Vector3(boxW, boxH, boxL),
        };

        int[] triangles = {
            0, 2, 1, // front
			1, 2, 3,
            4, 5, 6, // back
			5, 7, 6,
            6, 7, 8, //top
			7, 9 ,8,
            1, 3, 4, //bottom
			3, 5, 4,
            1, 11,10,// left
			1, 4, 11,
            3, 12, 5,//right
			5, 12, 13
        };

        Vector2[] uvs = {
            new Vector2(0, 0.625f),
            new Vector2(0.25f, 0.625f),
            new Vector2(0, 0.375f),
            new Vector2(0.25f, 0.375f),

            new Vector2(0.5f, 0.625f),
            new Vector2(0.5f, 0.375f),
            new Vector2(0.75f, 0.625f),
            new Vector2(0.75f, 0.375f),

            new Vector2(1, 0.625f),
            new Vector2(1, 0.375f),

            new Vector2(0.25f, 0.875f),
            new Vector2(0.5f, 0.875f),

            new Vector2(0.25f, 0.125f),
            new Vector2(0.5f, 0.125f)
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
