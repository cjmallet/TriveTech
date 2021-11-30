using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Part : MonoBehaviour
{
    [NamedListAttribute(new string[] { "Right", "Left", "Top", "Bottom", "Back", "Front" })]
    public List<Part> attachedParts = new List<Part>();
    public int Health { get; set; }
    public int Weight { get; set; }

    public int Width { get; }
    public int Height { get; }
    public int Speed { get; set; }

    //! Arrow object/mesh that is used to indicate the front direction
    public bool useDirectionIndicator;
    private GameObject directionIndicatorPrefab;
    private GameObject myDirectionIndicator;

    private const int SIDES = 6;

    public Part()
    {
        for (int i = 0; i < SIDES; i++)
        {
            attachedParts.Add(null);
        }
    }

    private void Awake()
    {
        if (useDirectionIndicator)
            ShowFrontDirection();
    }

    /// <summary>
    /// This ensures my part is attached to all of it's adjacent parts.
    /// </summary>
    /// <param name="partToAttachTo">Part that's seleted to connect to.</param>
    /// <param name="side">Side that's connecting.</param>
    public void AttachPart(Part partToAttachTo, Vector3 hitNormal)
    {
        /*
        // check who we attach to
        if ((int)side % 2 == 0)
        {
            // Add to my part list
            attachedParts[(int)side + 1] = partToAttachTo;
            // Set other side
            partToAttachTo.attachedParts[(int)side] = this;
        }
        else if (((int)side % 2 != 0))
        {
            attachedParts[(int)side - 1] = partToAttachTo;
            partToAttachTo.attachedParts[(int)side] = this;
        }
        */
        attachedParts[(int)DetermineSide(-hitNormal)] = partToAttachTo;
        partToAttachTo.attachedParts[(int)partToAttachTo.DetermineSide(hitNormal)] = this;

        // check if any adjacent parts also exist and add them to my list
    }

    public enum Orientation : int
    {
        Right,
        Left,
        Top,
        Bottom,
        Back,
        Front
    };

    /// <summary>
    /// Determines the side of the part that was clicked based on the hit normal. 
    /// The normal is
    /// </summary>
    /// <param name="normal">Hit normal</param>
    /// <returns></returns>
    public Orientation DetermineSide(Vector3 normal)
    {
        Vector3Int n = Vector3Int.RoundToInt(Quaternion.Inverse(transform.rotation) * normal);

        Debug.Log("rotated normal is: " + n);

        switch (n)
        {
            case Vector3Int v when v.Equals(Vector3Int.up):
                Debug.Log("Up");
                return Orientation.Top;
            case Vector3Int v when v.Equals(Vector3Int.left):
                Debug.Log("Left");
                return Orientation.Left;
            case Vector3Int v when v.Equals(Vector3Int.back):
                Debug.Log("Back");
                return Orientation.Back;
            case Vector3Int v when v.Equals(Vector3Int.forward):
                Debug.Log("Forward");
                return Orientation.Front;
            case Vector3Int v when v.Equals(Vector3Int.down):
                Debug.Log("Down");
                return Orientation.Bottom;
            case Vector3Int v when v.Equals(Vector3Int.right):
                Debug.Log("Right");
                return Orientation.Right;

            default:
                Debug.Log("NORMAL IS INCORRECT - ERROR ERROR ERROR");
                return Orientation.Right;//Not actually right at all
        }
    }

    /// <summary>
    /// Instantiates an arrow to indicate the direction this part is facing.
    /// </summary>
    public void ShowFrontDirection()
    {
        directionIndicatorPrefab = Resources.Load("DirectionIndicationArrow") as GameObject;
        myDirectionIndicator = Instantiate(directionIndicatorPrefab, this.transform.position, this.transform.rotation);
        myDirectionIndicator.transform.Translate(0, 0, 1, Space.Self);
        myDirectionIndicator.transform.Rotate(90, 0, 0);
        myDirectionIndicator.transform.SetParent(this.transform);
    }

    public void RemoveDirectionIndicator()
    {
        Destroy(myDirectionIndicator);
    }
}
