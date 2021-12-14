using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrowBoundingBox : MonoBehaviour
{
    public float MyArrowWidth { get; set; }
    public float MyArrowLength { get; set;}
    public float MyWidth { get; set; }
    public float MyLength { get; set; }

    void Start()
    {
        CreateArrow();
        SetPosition();
    }

    private void CreateArrow()
    {
        Vector3[] vertices = {

           // Create Arrow
            new Vector3(0, 0, 0),
            new Vector3(MyArrowWidth, 0, 0),
            new Vector3(0, 0, MyArrowLength),
            new Vector3(MyArrowWidth, 0, MyArrowLength),
        };

        int[] triangles = {
            0, 2, 1, 
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
        transform.localPosition = new Vector3((MyWidth / 2) - (MyArrowWidth / 2), 0, (MyLength / 2) - (MyArrowLength / 2));
    }
}
