using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGrid : MonoBehaviour
{
    [SerializeField] private Vector3Int gridDimensions;
    private Part[,,] partGrid;
    [SerializeField] private Vector3Int gridCenterIndex;//refactor to coreblock index when merge with desktop branch

    void Start()
    {
        partGrid = new Part[gridDimensions.x, gridDimensions.y, gridDimensions.z];
        gridCenterIndex = new Vector3Int(Mathf.CeilToInt((float)gridDimensions.x * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.y * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.z * 0.5f) - 1);
        partGrid[gridCenterIndex.x, gridCenterIndex.y, gridCenterIndex.z] = this.gameObject.GetComponent<Part>();//put coreblock in the center 
    }

    public void AddPartToGrid(Part partToAdd, Vector3Int relativePos)
    {
        if (CheckIfInBounds(relativePos))
        {
            Vector3Int gridIndex = relativePos + gridCenterIndex;
            partGrid[gridIndex.x, gridIndex.y, gridIndex.z] = partToAdd;
        }
        else
        {
            Debug.LogError("INDEX OUT OF BOUNDS");
        }
    }

    public void RemovePartFromGrid(Vector3Int relativePos)
    {
        if (CheckIfInBounds(relativePos))
        {
            Vector3Int gridIndex = relativePos + gridCenterIndex;
            partGrid[gridIndex.x, gridIndex.y, gridIndex.z] = null;
        }
        else
        {
            Debug.LogError("INDEX OUT OF BOUNDS");
        }
    }

    /// <summary>
    /// returns array of index's neighbours in this order: "Right", "Left", "Top", "Bottom", "Back", "Front"
    /// </summary>
    /// <param name="gridIndex"></param>
    /// <returns></returns>
    public Part[] GetNeighbours(Vector3Int relativePos)
    {
        Vector3Int gridIndex = relativePos + gridCenterIndex;
        Debug.Log("neighbour index: " + gridIndex);
        Part[] neighbours = new Part[6];
        //"Right", "Left", "Top", "Bottom", "Back", "Front"
        if (gridIndex.x + 1 <= partGrid.GetUpperBound(0) && partGrid[gridIndex.x + 1, gridIndex.y, gridIndex.z] != null)//TODO: null checks zouden weg kunnen. eerst even zo testen
        {
            neighbours[0] = partGrid[gridIndex.x + 1, gridIndex.y, gridIndex.z];
        }
        if (gridIndex.x - 1 >= partGrid.GetLowerBound(0) && partGrid[gridIndex.x - 1, gridIndex.y, gridIndex.z] != null)
        {
            neighbours[1] = partGrid[gridIndex.x - 1, gridIndex.y, gridIndex.z];
        }
        if (gridIndex.y + 1 <= partGrid.GetUpperBound(1) && partGrid[gridIndex.x, gridIndex.y + 1, gridIndex.z] != null)
        {
            neighbours[2] = partGrid[gridIndex.x, gridIndex.y + 1, gridIndex.z];
        }
        if (gridIndex.y - 1 >= partGrid.GetLowerBound(1) && partGrid[gridIndex.x, gridIndex.y - 1, gridIndex.z] != null)
        {
            neighbours[3] = partGrid[gridIndex.x, gridIndex.y - 1, gridIndex.z];
        }
        if (gridIndex.z + 1 <= partGrid.GetUpperBound(2) && partGrid[gridIndex.x, gridIndex.y, gridIndex.z + 1] != null)
        {
            neighbours[4] = partGrid[gridIndex.x, gridIndex.y, gridIndex.z + 1];
        }
        if (gridIndex.z - 1 >= partGrid.GetLowerBound(2) && partGrid[gridIndex.x, gridIndex.y, gridIndex.z - 1] != null)
        {
            neighbours[5] = partGrid[gridIndex.x, gridIndex.y, gridIndex.z - 1];
        }
        return neighbours;
    }
    /// <summary>
    /// Checks if a local position, relative to the core block, is within grid bounds
    /// </summary>
    /// <param name="relativePos">Local position to check, relative to the core block</param>
    /// <returns></returns>
    public bool CheckIfInBounds(Vector3Int relativePos)
    {
        Vector3Int gridIndex = relativePos + gridCenterIndex;
        if (gridIndex.x > partGrid.GetUpperBound(0) || gridIndex.x < partGrid.GetLowerBound(0) ||
            gridIndex.y > partGrid.GetUpperBound(1) || gridIndex.y < partGrid.GetLowerBound(1) ||
            gridIndex.z > partGrid.GetUpperBound(2) || gridIndex.z < partGrid.GetLowerBound(2))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

