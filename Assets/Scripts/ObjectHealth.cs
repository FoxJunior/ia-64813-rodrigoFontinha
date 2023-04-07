using UnityEngine;
using TMPro;

public class ObjectHealth : MonoBehaviour
{

    private int maxHealth = 100;
    private int currentHealth;

    public TMP_Text healthText;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthText.text = currentHealth + "/" + maxHealth;

    }

    private void UpdateBar()
    {
        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth - damage < 0)
            currentHealth = 0;
        else
            currentHealth -= damage;
        UpdateBar();
    }

    public void GiveHealth(int amount)
    {
        currentHealth += amount;
        UpdateBar();
    }

    public void ChangeMaxHealth(int amount)
    {
        currentHealth = amount;
        maxHealth = amount;
        healthBar.SetMaxHealth(maxHealth);
        UpdateBar();
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
