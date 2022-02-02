using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the arrow in the starting bounding box to be shown.
/// </summary>
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
    /// <summary>
    /// Creates the arrow indicating the direction where the player needs to build to 
    /// </summary>
    private void CreateArrow()
    {
        Vector3[] vertices = {
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

    /// <summary>
    /// Method that sets the arrows position to always be in the center of the bottom face of the boundingbox
    /// </summary>
    private void SetPosition()
    {
        transform.localPosition = new Vector3((MyWidth / 2) - (MyArrowWidth / 2), 0, (MyLength / 2) - (MyArrowLength / 2));
    }
}
