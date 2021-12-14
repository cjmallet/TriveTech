using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBorders : MonoBehaviour
{
    [SerializeField]
    private PerlinNoiseTerrain sandTerrain;
    public List<GameObject> borders;

    //! Adjust border height of sandbox
    public float borderHeight = 1.5f;

    //! Updates sizes of all 4 borders of the sandbox based on sand terrain size
    void FixedUpdate()
    {
        borders[0].transform.localScale = new Vector3(sandTerrain.xSize, borderHeight, 1);
        borders[1].transform.localScale = new Vector3(1, borderHeight, sandTerrain.zSize);
        borders[2].transform.localScale = new Vector3(sandTerrain.xSize, borderHeight, 1);
        borders[3].transform.localScale = new Vector3(1, borderHeight, sandTerrain.zSize);

        borders[0].transform.position = new Vector3(0, 1, (sandTerrain.zSize / 2) + 0.6f);
        borders[1].transform.position = new Vector3((sandTerrain.xSize / 2) + 0.6f, 1, 0);
        borders[2].transform.position = new Vector3(0, 1, (sandTerrain.zSize / -2) - 0.6f);
        borders[3].transform.position = new Vector3((sandTerrain.xSize / -2) - 0.6f, 1, 0);
    }
}
