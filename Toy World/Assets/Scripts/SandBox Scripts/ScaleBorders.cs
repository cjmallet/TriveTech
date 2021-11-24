using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBorders : MonoBehaviour
{
    [SerializeField]
    private PerlinNoiseTerrain sandTerrain;
    public List<GameObject> borders;

    //! Updates sizes of all 4 borders of the sandbox
    void FixedUpdate()
    {
        borders[0].transform.localScale = new Vector3(sandTerrain.xSize, 1.5f, 1);
        borders[1].transform.localScale = new Vector3(1, 1.5f, sandTerrain.zSize);
        borders[2].transform.localScale = new Vector3(sandTerrain.xSize, 1.5f, 1);
        borders[3].transform.localScale = new Vector3(1, 1.5f, sandTerrain.zSize);

        borders[0].transform.position = new Vector3(0, 1, sandTerrain.zSize / 2);
        borders[1].transform.position = new Vector3(sandTerrain.xSize / 2, 1, 0);
        borders[2].transform.position = new Vector3(0, 1, sandTerrain.zSize / -2);
        borders[3].transform.position = new Vector3(sandTerrain.xSize / -2, 1, 0);
    }
}
