using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A jump part that is a bouncy spring
/// </summary>
public class Spring : JumpPart
{
    private float t = 0;
    [SerializeField]
    private float strechyness = 0.75f;
    private bool isRetracting = false;
    private bool isFirstFrame = true;
    Vector3 defaultSpringPosition, defaultSpringScale, springPosition, springScale;

    /// <summary>
    /// Handles animation for the spring by scaling the object up and down using a lerp function
    /// </summary>
    public override void JumpAnimation()
    {
        if (isFirstFrame)
        {
            defaultSpringPosition = transform.GetChild(0).localPosition;
            defaultSpringScale = transform.GetChild(0).localScale;
            isFirstFrame = false;
        }

        if (isRetracting)
        {
            t -= Time.deltaTime * 2.5f;
            if (t <= 0)
            {
                isRetracting = false;
                PlayAnimation = false;
            }
        }
        else
        {
            t += Time.deltaTime * 4f;
            if (t >= strechyness)
                isRetracting = true;
        }

        float lerpValue = Mathf.Lerp(0, strechyness, t);

        springPosition = defaultSpringPosition;
        springPosition.y -= lerpValue;

        springScale = defaultSpringScale;
        springScale.y += lerpValue;

        transform.GetChild(0).localPosition = springPosition;
        transform.GetChild(0).localScale = springScale;
    }
}
