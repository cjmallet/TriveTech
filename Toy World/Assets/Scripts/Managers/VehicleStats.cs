using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleStats : MonoBehaviour
{
    private static VehicleStats instance;
    public static VehicleStats _instance
    {
        get
        {
            return instance;
        }
    }

    public List<Part> allParts = new List<Part>();

    public Canvas playerUI;
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public Gradient healthGradient;

    private int totalHealth { get; set; }
    private int currentHealth { get; set; }

    private void OnEnable()
    {
        if (instance == null) { instance = this; }

        if (allParts.Count != 0)
        {
            foreach (Part part in allParts)
            {
                totalHealth += part.health;
            }
        }
        currentHealth = totalHealth;

        healthText.text = (currentHealth + " / " + totalHealth);

        healthBar.fillAmount = currentHealth / totalHealth;
        healthBar.color = healthGradient.Evaluate(healthBar.fillAmount);

        playerUI.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        totalHealth = 0;
        currentHealth = 0;

        playerUI.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            healthText.text = (currentHealth + " / " + totalHealth);
            healthBar.fillAmount = (float)currentHealth / (float)totalHealth;
            healthBar.color = healthGradient.Evaluate(healthBar.fillAmount);
        }
        else
        {
            currentHealth = 0;
            healthText.text = (currentHealth + " / " + totalHealth);
            healthBar.fillAmount = (float)currentHealth / (float)totalHealth;
            healthBar.color = healthGradient.Evaluate(healthBar.fillAmount);
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("YOU DIED LOSER");
    }
}
