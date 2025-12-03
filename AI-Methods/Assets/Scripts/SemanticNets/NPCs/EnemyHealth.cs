using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public Transform healthBarPrefab;
    private Image fillImage;
    private Transform healthBarUI;

    private Transform playerCamera;

    void Start()
    {
        currentHealth = maxHealth;

        // Instantiate health bar UI
        healthBarUI = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);
        fillImage = healthBarUI.Find("Background/Fill").GetComponent<Image>();

        playerCamera = Camera.main.transform; 
    }

    void Update()
    {
        // Make health bar always face the camera
        healthBarUI.LookAt(playerCamera);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        // Update fill amount
        fillImage.fillAmount = (float)currentHealth / maxHealth;

        // Enemy dies
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
