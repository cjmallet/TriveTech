using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrowBoundingBox : BoundingBoxAndArrow
{
    void Start()
    {
        CreateArrow();
        SetPosition();
    }

    private void CreateArrow()
    {
        Vector3[] vertices = {

           // Create bounding box
            new Vector3(0, 0, 0),
            new Vector3(arrowW, 0, 0),
            new Vector3(0, 0, arrowL),
            new Vector3(arrowW, 0, arrowL),
        };

        int[] triangles = {
            0, 2, 1, // front
			1, 2, 3
        };

        Vector2[] uvs = {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),

           
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    private void SetPosition()
    {
        transform.localPosition = new Vector3((boxW / 2) - (arrowW / 2), 0, (boxL / 2) - (arrowL / 2));
    }
}
