using UnityEngine;
using TMPro;

public class PlayerPotion : MonoBehaviour
{

    private readonly int potionRestore = 40;
    public TMP_Text potionAmount;
    private int playerHas = 1;
    private ObjectHealth playerHealth;

    void Start()
    {
        playerHealth = transform.GetComponent<ObjectHealth>();
    }

    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T) && playerHas > 0)
        {
            int playerCurrentHealth = playerHealth.GetHealth();
            int playerMaxHealth = playerHealth.GetMaxHealth();
            if(playerCurrentHealth < playerMaxHealth)
            {
                if (playerCurrentHealth + potionRestore > playerMaxHealth)
                    playerHealth.GiveHealth(playerMaxHealth - playerCurrentHealth);
                else
                    playerHealth.GiveHealth(potionRestore);

                playerHas--;
                UpdateUI();
            }
        }
    }

    private void UpdateUI()
    {
        potionAmount.text = playerHas.ToString();
    }

    public void AddPotion()
    {
        playerHas++;
        UpdateUI();
    }

}
