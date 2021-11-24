using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTerrain : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    Vector2[] theUV;
    Vector3[] normals;
    Vector4[] tangents;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    [Range(0, 0.5f)]
    public float noiseEffectiveness = .1f;
    public float offsetX = 100, offsetY = 100;
    public bool isMovingTerrain = false;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
        CreateMeshShape();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CreateMeshShape();
        if (isMovingTerrain)
        {
            offsetX += Time.deltaTime;
            offsetY += Time.deltaTime;
        }
        UpdateMesh();
    }

    void CreateMeshShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        DrawVertices();

        triangles = new int[xSize * zSize * 6];

        int verticeNumber = 0;
        int triangleNumber = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[triangleNumber + 0] = verticeNumber + 0;
                triangles[triangleNumber + 1] = verticeNumber + xSize + 1;
                triangles[triangleNumber + 2] = verticeNumber + 1;
                triangles[triangleNumber + 3] = verticeNumber + 1;
                triangles[triangleNumber + 4] = verticeNumber + xSize + 1;
                triangles[triangleNumber + 5] = verticeNumber + xSize + 2;

                verticeNumber++;
                triangleNumber += 6;
            }

            verticeNumber++;
        }
    }

    void DrawVertices()
    {
        theUV = new Vector2[vertices.Length];
        tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(0, 0, 1, -1);
        for (int iVertices = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * noiseEffectiveness + offsetX, z * noiseEffectiveness + offsetY) * 2;
                vertices[iVertices] = new Vector3(x - xSize / 2, y, z - zSize / 2);
                theUV[iVertices] = new Vector2((float)x / xSize, (float)z / zSize);
                tangents[iVertices] = tangent;
                iVertices++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.uv = theUV;
        mesh.tangents = tangents;
        mesh.triangles = triangles;

        mesh.normals = normals;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.yellow;
        for (int iVertices = 0; iVertices < vertices.Length; iVertices++)
        {
            Gizmos.DrawSphere(vertices[iVertices], .1f);
        }
    }
}

