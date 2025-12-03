using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player died");
    }
}