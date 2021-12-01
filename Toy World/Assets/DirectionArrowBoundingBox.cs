using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrowBoundingBox : BoundingBoxAndArrow
{
    void Start()
    {
        CreateBoundingBoxWithArrow();
    }

    private void CreateBoundingBoxWithArrow()
    {
        Vector3[] vertices = {

           
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
            new Vector2(0, (2f / 3f)),
            new Vector2(0.25f, (2f / 3f)),
            new Vector2(0, (1f / 3f)),
            new Vector2(0.25f, (1f / 3f)),

            new Vector2(0.5f, (2f / 3f)),
            new Vector2(0.5f, (1f / 3f)),
            new Vector2(0.75f, (2f / 3f)),
            new Vector2(0.75f, (1f / 3f)),

            new Vector2(1, (2f / 3f)),
            new Vector2(1, (1f / 3f)),

            new Vector2(0.25f, 1),
            new Vector2(0.5f, 1),

            new Vector2(0.25f, 0),
            new Vector2(0.5f, 0),
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
