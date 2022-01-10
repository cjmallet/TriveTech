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

    // Start is called before the first frame update
    void Start()
    {
        foreach (Part part in allParts)
        {
            currentWeight += (int)part.weight;
            if (part.GetType() == typeof(MovementPart))
                currentTorque += (int)part.GetComponent<MovementPart>().maxTorgue;
        }

        foreach (Transform child in transform)
        {
            if (child.childCount > 0) // Only children with children
            {
                SetStat(child.gameObject);
            }
        }
    }

    private void SetStat(GameObject stat)
    {
        Image image = stat.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        TextMeshProUGUI text = stat.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        if (currentWeight == 0)
            currentWeight = 10;

        switch (stat.name)
        {
            case "Mass":
                image.color = statGradient.Evaluate((float)currentWeight / 1000f);
                text.text = currentWeight.ToString() + " kg";
                break;
            case "Torque":
                image.color = speedGradient.Evaluate((float)currentTorque / ((float)currentWeight * 10f)); // 2000 is max torque possible
                text.text = currentTorque.ToString() + " nm";
                break;
            case "TopSpeed":
                float power = (currentTorque * 600f / 9.5488f);
                float kmh = (power / currentWeight / 9.81f) * 3.6f;
                image.color = speedGradient.Evaluate(kmh / 100);
                text.text = kmh.ToString() + " km/h";
                break;
            default:
                // code
                break;
        }
    }
}