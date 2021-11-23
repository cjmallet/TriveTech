using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPart : Part
{
    public bool connectingPart = false;
    private int health = 75;

    public TestPart()
    {
        this.Health = health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (connectingPart)
        {
            AttachPart(collision.transform.GetComponent<Part>(), Orientation.Back);
            connectingPart = false;
        }
    }
}
