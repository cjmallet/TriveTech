using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPart : Part
{
    private int health = 10;
    private float myMass = 0.2f;

    public TestPart()
    {
        this.Health = health;
        this.Weight = myMass;
    }
}
