using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Part : MonoBehaviour
{
    public List<Part> attachedParts = new List<Part>();
    public int Health { get; set; }
    public int Weight { get; set; }

    public int Width { get; }
    public int Height { get; }

    public Part()
    {

    }

    /// <summary>
    /// This ensures my part is attached to all of it's adjacent parts.
    /// </summary>
    /// <param name="partToAttachTo">Part that's seleted to connect to.</param>
    public void AttachPart(Part partToAttachTo, Orientation side)
    {
        // check who we attach to
        if ((int)side % 2 == 0)
        {
            // Set my side
            attachedParts[(int)side] = partToAttachTo;
            // Set other side
            partToAttachTo.attachedParts[(int)side + 1] = this;
        }
        else
        {
            attachedParts[(int)side] = partToAttachTo;
            partToAttachTo.attachedParts[(int)side - 1] = this;
        }
        // check if any adjacent parts also exist and add them to my list
    }

    public enum Orientation : int
    {
        Top,
        Bottom,
        Back,
        Front,
        Left,
        Right
    };
}
