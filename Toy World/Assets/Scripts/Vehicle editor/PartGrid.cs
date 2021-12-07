using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartGrid : MonoBehaviour
{
    [SerializeField] private Vector3Int gridDimensions;
    private Part[,,] partGrid;
    [SerializeField] private Vector3Int coreBlockIndex;//refactor to coreblock index when merge with desktop branch
    [SerializeField] private GameObject cubePrefab;
    private GameObject tempBox;

    void Start()
    {
        partGrid = new Part[gridDimensions.x, gridDimensions.y, gridDimensions.z];
        coreBlockIndex = new Vector3Int(Mathf.CeilToInt((float)gridDimensions.x * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.y * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.z * 0.5f) - 1);
        partGrid[coreBlockIndex.x, coreBlockIndex.y, coreBlockIndex.z] = this.gameObject.GetComponent<Part>();//put coreblock in the center 
        tempBox = Instantiate(cubePrefab, this.transform);
        ToggleTempBoundingBox(true);
    }

    public void AddPartToGrid(Part partToAdd, Vector3Int relativePos)
    {
        if (CheckIfInBounds(relativePos))
        {
            Vector3Int gridIndex = relativePos + coreBlockIndex;
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
            Vector3Int gridIndex = relativePos + coreBlockIndex;
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
        Vector3Int gridIndex = relativePos + coreBlockIndex;
        //Debug.Log("neighbour index: " + gridIndex);
        //Debug.Log("neighbour index: " + gridIndex);
        Part[] neighbours = new Part[6];
        //"Right", "Left", "Top", "Bottom", "Back", "Front"
        if (gridIndex.x + 1 <= partGrid.GetUpperBound(0))
        {
            neighbours[0] = partGrid[gridIndex.x + 1, gridIndex.y, gridIndex.z];
        }
        if (gridIndex.x - 1 >= partGrid.GetLowerBound(0))
        {
            neighbours[1] = partGrid[gridIndex.x - 1, gridIndex.y, gridIndex.z];
        }
        if (gridIndex.y + 1 <= partGrid.GetUpperBound(1))
        {
            neighbours[2] = partGrid[gridIndex.x, gridIndex.y + 1, gridIndex.z];
        }
        if (gridIndex.y - 1 >= partGrid.GetLowerBound(1))
        {
            neighbours[3] = partGrid[gridIndex.x, gridIndex.y - 1, gridIndex.z];
        }
        if (gridIndex.z + 1 <= partGrid.GetUpperBound(2))
        {
            neighbours[4] = partGrid[gridIndex.x, gridIndex.y, gridIndex.z + 1];
        }
        if (gridIndex.z - 1 >= partGrid.GetLowerBound(2))
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
        Vector3Int gridIndex = relativePos + coreBlockIndex;
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



    /*
     * 
     * 
     * MAAK 6 LOOPS VOOR ELKE RICHTING
     * hij laat blokken achter omdat je op negatief of positief checkt. dan skip je de edge planes
     * doe de checks dan gewoon in de loop zelf.
     * 
     * 
     */ 
    /// <summary>
    /// Moves the vehicle in the grid by 1 step in a single direction
    /// </summary>
    /// <param name="movement"></param>
    public void MoveVehicleInGrid(InputAction.CallbackContext context)
    {
        Vector3Int movement = Vector3Int.RoundToInt(context.ReadValue<Vector3>());
        if (movement.magnitude == 1)//checks if it's just 1 step
        {
            //the 3 loops below check if the outer plane of the direction in which we're moving is clear. It can't move outside of the grid.
            bool weGood = true;

            if (movement.x != 0)
            {
                int x = 999999;//alles kapot brrrrah brah brah
                if (movement.x == 1)
                {
                    x = partGrid.GetUpperBound(0);
                }
                else if (movement.x == -1)
                {
                    x = 0;
                }
                else
                    Debug.LogError("xit's all fucked, screw this I'm out");


                for (int i = 0; i < partGrid.GetUpperBound(1); i++)
                {
                    for (int j = 0; j < partGrid.GetUpperBound(2); j++)
                    {
                        if (partGrid[x, i, j] != null)
                        {
                            Debug.Log($"X axis blocked x{x} y{i} z{j} {partGrid[x, i, j]}");
                            weGood = false;
                            break;
                        }
                    }
                    if (!weGood)
                        break;
                }
            }
            else if (movement.y != 0)
            {
                int y = 99999;
                if (movement.y == 1)
                    y = partGrid.GetUpperBound(1);
                else if (movement.y == -1)
                    y = 0;
                else
                    Debug.LogError("yit's all fucked");

                for (int i = 0; i < partGrid.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < partGrid.GetUpperBound(2); j++)
                    {
                        if (partGrid[i, y, j] != null)
                        {
                            Debug.Log($"Y axis blocked {partGrid[i, y, j]}");
                            weGood = false;
                            break;
                        }

                    }
                    if (!weGood)
                        break;
                }
            }
            else if (movement.z != 0)
            {
                int z = 99999;
                if (movement.z == 1)
                    z = partGrid.GetUpperBound(2);
                else if (movement.z == -1)
                    z = 0;
                else
                    Debug.LogError("zit's all fucked, screw this I'm out");

                for (int i = 0; i < partGrid.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < partGrid.GetUpperBound(1); j++)
                    {
                        if (partGrid[i, j, z] != null)
                        {
                            Debug.Log($"Z axis blocked {partGrid[i, j, z]}");
                            weGood = false;
                            break;
                        }

                    }
                    if (!weGood)
                        break;
                }
            }

            // Below here is where the parts are actually moved, either in a regular or a reverse loop, depending on movement direction
            // I could also do the above checks during the first loop over a plane, but I would still need 6 different nested for loops for each direction...
            // actually I'll do that, it's a much neater solution when I think about it. It could expand to multiple steps and checks of multiple layers.
            if (weGood && (movement.x == -1 || movement.y == -1 || movement.z == -1))
            {
                Debug.Log($"weGood is {weGood} movement is {movement} and we are moving now.");
                for (int i = 1; i < partGrid.GetUpperBound(0); i++)
                {
                    for (int j = 1; j < partGrid.GetUpperBound(1); j++)
                    {
                        for (int k = 1; k < partGrid.GetUpperBound(2); k++)
                        {
                            if (partGrid[i, j, k] != null)
                            {
                                partGrid[i + movement.x, j + movement.y, k + movement.z] = partGrid[i, j, k];
                                partGrid[i, j, k] = null;
                            }
                        }
                    }
                }
                coreBlockIndex += movement;
                Debug.Log($"coreblockIndex is occupied by: {partGrid[coreBlockIndex.x, coreBlockIndex.y, coreBlockIndex.z].transform.name}");
                ToggleTempBoundingBox(true);
            }
            else if (weGood && (movement.x == 1 || movement.y == 1 || movement.z == 1))
            {
                Debug.Log($"weGood is {weGood} movement is {movement} and we are moving now.");
                for (int i = partGrid.GetUpperBound(0); i >= 0; i--)
                {
                    for (int j = partGrid.GetUpperBound(1); j >= 0; j--)
                    {
                        for (int k = partGrid.GetUpperBound(2); k >= 0; k--)
                        {
                            if (partGrid[i, j, k] != null)
                            {
                                partGrid[i + movement.x, j + movement.y, k + movement.z] = partGrid[i, j, k];
                                partGrid[i, j, k] = null;
                            }
                        }
                    }
                }
                coreBlockIndex += movement;
                Debug.Log($"coreblockIndex is occupied by: {partGrid[coreBlockIndex.x, coreBlockIndex.y, coreBlockIndex.z].transform.name}");
                ToggleTempBoundingBox(true);
            }
        }
    }



    public void ToggleTempBoundingBox(bool active)
    {
        if (active)
        {
            tempBox.transform.localPosition = new Vector3(gridDimensions.x * 0.5f - 0.5f, gridDimensions.y * 0.5f - 0.5f, gridDimensions.z * 0.5f - 0.5f) - coreBlockIndex;
            tempBox.transform.localScale = gridDimensions;
        }
        else
        {
            tempBox.SetActive(false);
        }
    }

    public void ChangeGridSize(Vector3Int newDimensions)
    {

        /*
        for (int j = i; j < length; j++)
        {

        }
        */

        gridDimensions = newDimensions;

        Part[,,] newPartArray = new Part[gridDimensions.x, gridDimensions.y, gridDimensions.z];
        //Vector3Int offset = new Vector3Int(Mathf.CeilToInt((float)gridDimensions.x * 0.5f) - 1

        for (int i = 0; i < newPartArray.GetLength(0); i++)
        {
            for (int j = 0; j < newPartArray.GetLength(0); j++)
            {
                for (int k = 0; i < newPartArray.GetLength(0); k++)
                {

                    //Move the parts from the current array to the new array, relative to where they were
                    //example: Old array size 20, new array size 30.Block on index 8 would move to index 13, so it looks like 5 spaces are added in each direction
                }
            }
        }
        gridDimensions = newDimensions;
    }
    void OnDrawGizmos()
    {
        if (partGrid != null)
        {
            for (int i = 0; i < partGrid.GetUpperBound(0); i++)
            {
                for (int j = 0; j < partGrid.GetUpperBound(1); j++)
                {
                    for (int k = 0; k < partGrid.GetUpperBound(2); k++)
                    {
                        if (partGrid[i, j, k] != null)
                            Gizmos.color = Color.red;
                        else
                            Gizmos.color = Color.black;

                        Gizmos.DrawSphere(new Vector3(i, j + 10, k), 0.1f);
                    }
                }
            }
        }
    }
}

