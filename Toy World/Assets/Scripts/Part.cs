using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Part : MonoBehaviour
{
    [NamedListAttribute (new string[] {  "Right", "Left", "Top", "Bottom", "Back", "Front"})]
    public List<Part> attachedParts = new List<Part>();
    public int Health { get; set; }
    public int Weight { get; set; }

    public int Width { get; }
    public int Height { get; }

    private const int SIDES = 6;

    public Part()
    {
        for (int i = 0; i < SIDES; i++)
        {
            attachedParts.Add(null);
        }
    }

    /// <summary>
    /// This ensures my part is attached to all of it's adjacent parts.
    /// </summary>
    /// <param name="partToAttachTo">Part that's seleted to connect to.</param>
    /// <param name="side">Side that's connecting.</param>
    public void AttachPart(Part partToAttachTo, Orientation side)
    {
        // check who we attach to
        if ((int)side % 2 == 0)
        {
            // Add to my part list
            attachedParts[(int)side + 1] = partToAttachTo;
            // Set other side
            partToAttachTo.attachedParts[(int)side] = this;
        }
        else
        {
            attachedParts[(int)side - 1] = partToAttachTo;
            partToAttachTo.attachedParts[(int)side] = this;
        }
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
}
