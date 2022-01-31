using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class StatWindowUI : MonoBehaviour
{
    public Gradient statGradient, speedGradient;
    public List<Part> allParts = new List<Part>();

    private int currentWeight = 0, currentTorque = 0;

    public void SetupAllParts(int movePartCount)
    {
        currentTorque = 0;
        currentWeight = 0;

        foreach (Part part in allParts)
        {
            currentWeight += (int)part.weight;

            if (part is MovementPart)
            {
                currentTorque += (int)part.GetComponent<MovementPart>().maxTorgue;
            }
        }

        foreach (Transform child in transform)
        {
            if (child.childCount > 0) // Only children with children
            {
                SetStat(child.gameObject, movePartCount);
            }
        }
    }

    public void UpdateStats(Part updatedPart, bool removed, int movePartCount)
    {
        if (updatedPart is MovementPart && !removed)
            currentTorque += (int)updatedPart.GetComponent<MovementPart>().maxTorgue;
        else if (updatedPart is MovementPart && removed)
            currentTorque -= (int)updatedPart.GetComponent<MovementPart>().maxTorgue;

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

    private void SetStat(GameObject stat, int movePartCount)
    {
        Image image;
        TextMeshProUGUI text;

        if (currentWeight == 0)
            currentWeight = 10;

        switch (stat.name)
        {
            case "Mass":
                image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                float mass = (float)currentWeight / 1000f;
                image.color = statGradient.Evaluate(mass);
                image.transform.GetComponentInChildren<Image>().fillAmount= mass;
                break;
            case "Acceleration":
                image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                float acceleration = (float)currentTorque / ((float)currentWeight * 10f);
                image.color = speedGradient.Evaluate(acceleration); // 2000 is max torque possible
                image.transform.GetComponentInChildren<Image>().fillAmount = acceleration;
                break;
            case "Speed":
                image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                float power = (float)(currentTorque / (currentWeight*5f));
                image.color = speedGradient.Evaluate(power);
                image.transform.GetComponentInChildren<Image>().fillAmount = power;
                break;
            case "WheelCap":
                text = stat.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = "Wheels " + movePartCount + " / 20";
                break;
            default:
                // code
                break;
        }
    }
}