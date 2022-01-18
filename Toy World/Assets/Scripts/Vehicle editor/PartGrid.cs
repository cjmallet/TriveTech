using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartGrid : MonoBehaviour
{
    public bool gizmos;
    [SerializeField] private Vector3Int gridDimensions;
    [SerializeField] private Part[,,] partGrid;
    [SerializeField] private Vector3Int coreBlockIndex;//refactor to coreblock index when merge with desktop branch
    //[SerializeField] private GameObject cubePrefab,tempBox;
    public GameObject _boundingBox;

    [HideInInspector]
    public List<Part> allParts = new List<Part>();

    void Start()
    {
        if(partGrid == null)
        {
            partGrid = new Part[gridDimensions.x, gridDimensions.y, gridDimensions.z];
            coreBlockIndex = new Vector3Int(Mathf.CeilToInt((float)gridDimensions.x * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.y * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.z * 0.5f) - 1);
            partGrid[coreBlockIndex.x, coreBlockIndex.y, coreBlockIndex.z] = this.gameObject.GetComponent<Part>();//put coreblock in the center 
        }

        if (_boundingBox == null)
            InstantiateBoundingBoxWithGridSize();
    }

    public void RemakePartGrid()
    {
        partGrid = new Part[gridDimensions.x, gridDimensions.y, gridDimensions.z];
       
        //coreBlockIndex = new Vector3Int(Mathf.CeilToInt((float)gridDimensions.x * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.y * 0.5f) - 1, Mathf.CeilToInt((float)gridDimensions.z * 0.5f) - 1);
        partGrid[coreBlockIndex.x, coreBlockIndex.y, coreBlockIndex.z] = this.gameObject.GetComponent<Part>();//put coreblock in the center 
        //List<Part> parts = new List<Part>();
        foreach (Part part in transform.GetComponentsInChildren<Part>())
        {
            if (part.transform != transform)
            {
                Vector3Int index = Vector3Int.RoundToInt(part.transform.localPosition) + coreBlockIndex;
                partGrid[index.x, index.y, index.z] = part;
            }

        }
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

    public List<Part> ReturnAllParts()
    {
        //List<Part> allParts = new List<Part>();

        if (allParts.Count != 0)
            allParts.Clear();

        for (int i = 0; i <= partGrid.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= partGrid.GetUpperBound(1); j++)
            {
                for (int k = 0; k <= partGrid.GetUpperBound(2); k++)
                {
                    if (partGrid[i, j, k] != null)
                    {
                        allParts.Add(partGrid[i, j, k]);
                    }
                }
            }
        }
        return allParts;
    }

    public void RemovePart(Vector3Int partPosition)
    {
        partGrid[coreBlockIndex.x + partPosition.x, coreBlockIndex.y + partPosition.y, coreBlockIndex.z + partPosition.z] = null;
    }

    /// <summary>
    /// Checks if any block is not connected to the vehicle through an list of all parts
    /// which is checked by the floodfill algorithm
    /// </summary>
    public void CheckConnection()
    {
        FloodFill();
        allParts = ReturnAllParts();

        foreach (Part part in allParts)
        {
            if (!part.floodFilled)
            {
                part.RemovePart(false);
            }
            else
            {
                part.floodFilled = false;
            }
        }
    }

    /// <summary>
    /// The floodfill algorithm checks each neighbour and turns the floodfill boolean to true
    /// if it is still connected to the coreblock. Works like the paint bucket tool.
    /// </summary>
    private void FloodFill()
    {
        Queue<Part> partsToCheck = new Queue<Part>();
        List<Part> neighboursCoreBlock= partGrid[coreBlockIndex.x,coreBlockIndex.y,coreBlockIndex.z].attachedParts;

        foreach (Part neighbour in neighboursCoreBlock)
        {
            if (neighbour!=null)
            {
                neighbour.floodFilled = true;
                partsToCheck.Enqueue(neighbour);
            }
        }

        while (partsToCheck.Count>0)
        {
            List<Part> neighbours = partsToCheck.Dequeue().attachedParts;

            foreach (Part neighbour in neighbours)
            {
                if (neighbour!=null&&!neighbour.floodFilled)
                {
                    neighbour.floodFilled = true;
                    partsToCheck.Enqueue(neighbour);
                }
            }
        }
    }

    /// <summary>
    /// Moves the vehicle over the grid. Can take multiple axes at once, as long as they're 1 or -1
    /// </summary>
    /// <param name="context">input</param>
    public void MoveVehicleInGrid(InputAction.CallbackContext context)
    {
        Vector3Int movement = Vector3Int.RoundToInt(context.ReadValue<Vector3>());
        if (movement != Vector3.zero)
        {
            if (movement.x == 1)
            {
                for (int x = partGrid.GetUpperBound(0); x >= 0; x--)
                {
                    for (int y = partGrid.GetUpperBound(1); y >= 0; y--)
                    {
                        for (int z = partGrid.GetUpperBound(2); z >= 0; z--)
                        {
                            if (partGrid[x, y, z] != null)
                            {
                                if (x == partGrid.GetUpperBound(0))
                                {
                                    //cannot go further on X
                                    //Debug.Log($"X axis blocked x{x} y{y} z{z}");
                                    goto LoopEnd;
                                }
                                partGrid[x + movement.x, y, z] = partGrid[x, y, z];
                                partGrid[x, y, z] = null;
                            }
                        }
                    }
                }
                coreBlockIndex.x += movement.x;
                //ToggleTempBoundingBox(true);
            LoopEnd:;
            }
            else if (movement.x == -1)
            {
                for (int x = 0; x <= partGrid.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= partGrid.GetUpperBound(1); y++)
                    {
                        for (int z = 0; z <= partGrid.GetUpperBound(2); z++)
                        {
                            if (partGrid[x, y, z] != null)
                            {
                                if (x == 0)
                                {
                                    //cannot go further on -X
                                    goto LoopEnd;
                                }
                                partGrid[x + movement.x, y, z] = partGrid[x, y, z];
                                partGrid[x, y, z] = null;
                            }
                        }
                    }
                }
                coreBlockIndex.x += movement.x;
                //ToggleTempBoundingBox(true);
            LoopEnd:;
            }
            if (movement.y == 1)
            {
                for (int y = partGrid.GetUpperBound(1); y >= 0; y--)
                {
                    for (int x = partGrid.GetUpperBound(0); x >= 0; x--)
                    {
                        for (int z = partGrid.GetUpperBound(2); z >= 0; z--)
                        {
                            if (partGrid[x, y, z] != null)
                            {
                                if (y == partGrid.GetUpperBound(1))
                                {
                                    
                                    goto LoopEnd;
                                }
                                partGrid[x, y + movement.y, z] = partGrid[x, y, z];
                                partGrid[x, y, z] = null;
                            }
                        }
                    }
                }
                coreBlockIndex.y += movement.y;
                //ToggleTempBoundingBox(true);
            LoopEnd:;
            }
            else if (movement.y == -1)
            {
                for (int y = 0; y <= partGrid.GetUpperBound(1); y++)
                {
                    for (int x = 0; x <= partGrid.GetUpperBound(0); x++)
                    {
                        for (int z = 0; z <= partGrid.GetUpperBound(2); z++)
                        {
                            if (partGrid[x, y, z] != null)
                            {
                                if (y == 0)
                                {
                                    //cannot go further on -Y
                                    goto LoopEnd;
                                }
                                partGrid[x, y + movement.y, z] = partGrid[x, y, z];
                                partGrid[x, y, z] = null;
                            }
                        }
                    }
                }
                coreBlockIndex.y += movement.y;
                //ToggleTempBoundingBox(true);
            LoopEnd:;
            }
            if (movement.z == 1)
            {
                for (int z = partGrid.GetUpperBound(2); z >= 0; z--)
                {
                    for (int x = partGrid.GetUpperBound(0); x >= 0; x--)
                    {
                        for (int y = partGrid.GetUpperBound(1); y >= 0; y--)
                        {
                            if (partGrid[x, y, z] != null)
                            {
                                if (z == partGrid.GetUpperBound(2))
                                {
                                    //cannot go further on Z
                                    goto LoopEnd;
                                }
                                partGrid[x, y, z + movement.z] = partGrid[x, y, z];
                                partGrid[x, y, z] = null;
                            }
                        }
                    }
                }
                coreBlockIndex.z += movement.z;
                //ToggleTempBoundingBox(true);
            LoopEnd:;
            }
            else if (movement.z == -1)
            {
                for (int z = 0; z <= partGrid.GetUpperBound(2); z++)
                {
                    for (int x = 0; x <= partGrid.GetUpperBound(0); x++)
                    {
                        for (int y= 0; y <= partGrid.GetUpperBound(1); y++)
                        {
                            if (partGrid[x, y, z] != null)
                            {
                                if (z == 0)
                                {
                                    //cannot go further on -Z
                                    goto LoopEnd;
                                }
                                partGrid[x, y, z + movement.z] = partGrid[x, y, z];
                                partGrid[x, y, z] = null;
                            }
                        }
                    }
                }
                coreBlockIndex.z += movement.z;
                //ToggleTempBoundingBox(true);
            LoopEnd:;
            }
        }
    }

    //public void ToggleTempBoundingBox(bool active)
    //{
    //    if (active)
    //    {
    //        tempBox.SetActive(true);
    //        tempBox.transform.localPosition = new Vector3(gridDimensions.x * 0.5f - 0.5f, gridDimensions.y * 0.5f - 0.5f, gridDimensions.z * 0.5f - 0.5f) - coreBlockIndex;
    //        tempBox.transform.localScale = gridDimensions;
    //    }
    //    else
    //    {
    //        tempBox.SetActive(false);
    //    }
    //}

    public void ToggleBoundingBox(bool state)
    {
        _boundingBox.SetActive(state);
    }

    public void InstantiateBoundingBoxWithGridSize()
    {
        GameObject boundingBoxAndArrow = Resources.Load("BoundingBoxWithDirectionArrow") as GameObject;
        boundingBoxAndArrow.GetComponent<BoundingBoxAndArrow>().boxW = gridDimensions.x;
        boundingBoxAndArrow.GetComponent<BoundingBoxAndArrow>().boxH = gridDimensions.y;
        boundingBoxAndArrow.GetComponent<BoundingBoxAndArrow>().boxL = gridDimensions.z;
        
        _boundingBox = Instantiate(Resources.Load("BoundingBoxWithDirectionArrow") as GameObject, transform);
        _boundingBox.transform.Translate(new Vector3(transform.position.x - (_boundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxW * 0.5f) - 0.1f,
                                                    transform.position.y - _boundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxH,//waarom zijn deze waardes allemaal anders?
                                                    transform.position.z - (_boundingBox.GetComponentInChildren<BoundingBoxAndArrow>().boxL * 0.5f) - 1f));
    }

    public void ChangeGridSize(Vector3Int newDimensions)
    {
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
        if (gizmos && partGrid != null)
        {
            for (int i = 0; i <= partGrid.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= partGrid.GetUpperBound(1); j++)
                {
                    for (int k = 0; k <= partGrid.GetUpperBound(2); k++)
                    {
                        if (partGrid[i, j, k] != null)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(new Vector3(i, j + 10, k) * 0.5f, Vector3.one * 0.45f);
                        }
                        else
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawWireCube(new Vector3(i, j + 10, k) *0.5f, Vector3.one * 0.5f);
                        }
                    }
                }
            }
        }
    }
}

