using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Stat window that shows current vehicle stats.
/// </summary>
public class StatWindowUI : MonoBehaviour
{
    public Gradient statGradient, speedGradient;
    public List<Part> allParts = new List<Part>();

    private int currentWeight = 0, currentTorque = 0;

    /// <summary>
    /// Called when all current vehicle parts need to be added to stat window.
    /// </summary>
    /// <param name="movePartCount">Amount of movement parts currently on the vehicle.</param>
    public void SetupAllParts(int movePartCount)
    {
        currentTorque = 0; // Reset stats when applying ALL parts again.
        currentWeight = 0;

        foreach (Part part in allParts) // Add all part stats.
        {
            currentWeight += (int)part.weight;

            if (part is MovementPart)
            {
                currentTorque += (int)part.GetComponent<MovementPart>().maxTorgue;
            }
        }

        foreach (Transform child in transform) // Apply all stats to all possible UI shown stats.
        {
            if (child.childCount > 0) // Only children with children
            {
                SetStat(child.gameObject, movePartCount);
            }
        }
    }

    /// <summary>
    /// Called to update the stats WHILE building the vehicle.
    /// </summary>
    /// <param name="updatedPart">The part that was placed or removed.</param>
    /// <param name="removed">True is removed.</param>
    /// <param name="movePartCount">Amount of movement parts.</param>
    public void UpdateStats(Part updatedPart, bool removed, int movePartCount)
    {
        if (updatedPart is MovementPart && !removed)
            currentTorque += (int)updatedPart.GetComponent<MovementPart>().maxTorgue; // Add stats to total.
        else if (updatedPart is MovementPart && removed)
            currentTorque -= (int)updatedPart.GetComponent<MovementPart>().maxTorgue; // Remove stats from total.

        if (!removed)
            currentWeight += (int)updatedPart.weight;
        else
            currentWeight -= (int)updatedPart.weight;

        foreach (Transform child in transform)
        {
            if (child.childCount > 0) // Only children with children
            {
                SetStat(child.gameObject, movePartCount);
            }
        }
    }

    /// <summary>
    /// Sets up a stat in the UI by using the collected data.
    /// </summary>
    /// <param name="stat">UI element of the stat.</param>
    /// <param name="movePartCount">Amount of movement parts.</param>
    private void SetStat(GameObject stat, int movePartCount)
    {
        Image image;
        TextMeshProUGUI text;

        if (currentWeight == 0) // If somehow the coreblock is not found or calculated due to Dont Destroy on Load, still count it.
            currentWeight = 10;

        switch (stat.name)
        {
            case "Mass": // Weight stat
                image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                float mass = (float)currentWeight / 1000f;
                image.color = statGradient.Evaluate(mass);
                image.transform.GetComponentInChildren<Image>().fillAmount= mass;
                break;
            case "Acceleration": // Accel stat
                image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                float acceleration = (float)currentTorque / ((float)currentWeight * 10f);
                image.color = speedGradient.Evaluate(acceleration); // 2000 is max torque possible
                image.transform.GetComponentInChildren<Image>().fillAmount = acceleration;
                break;
            case "Speed": // Top speed stat
                image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                float power = (float)(currentTorque / (currentWeight*5f));
                image.color = speedGradient.Evaluate(power);
                image.transform.GetComponentInChildren<Image>().fillAmount = power;
                break;
            case "WheelCap": // Amount of wheels compared to the cap
                text = stat.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = "Wheels " + movePartCount + " / 20";
                break;
            default:
                // code
                break;
        }
    }
}