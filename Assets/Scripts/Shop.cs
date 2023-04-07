using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{

    public TMP_Text warnText, levelText;

    private readonly bool[] standings = new bool[3];
    private readonly string[] colliders = new string[] {"Upgrades", "Bullets", "Potions"};
    private Shoot shoot;
    private PlayerPotion playerPotion;
    private PlayerCollectables playerColl;
    private readonly int upgradePrice = 50, bulletPrice = 100, bandagePrice = 200;
    private int upgradeLevel = 1;

    void Start()
    {
        for (int i = 0; i < 3; i++) standings[i] = false;
        playerColl = transform.GetComponent<PlayerCollectables>();
        shoot = transform.GetComponent<Shoot>();
        playerPotion = transform.GetComponent<PlayerPotion>();
    }

    void Update()
    {
        
        if((standings[0] || standings[1] || standings[2]) && Input.GetKeyUp(KeyCode.E))
        {
            int price = standings[0] ? upgradePrice * upgradeLevel : standings[1] ? bulletPrice : bandagePrice;
            if(playerColl.GetBalance() >= price)
            {
                playerColl.TakeBalance(price);
                if (standings[0]) Upgrade();
                if (standings[1]) BuyBullets();
                if (standings[2]) BuyPotions();

            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        bool upgrades = other.gameObject.name.Equals(colliders[0]);
        bool bullets = other.gameObject.name.Equals(colliders[1]);
        bool bandages = other.gameObject.name.Equals(colliders[2]);

        if(upgrades || bullets || bandages)
        {
            standings[(upgrades ? 0 : bullets ? 1 : 2)] = true;
            UpdateText();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        bool upgrades = other.gameObject.name.Equals(colliders[0]);
        bool bullets = other.gameObject.name.Equals(colliders[1]);
        bool bandages = other.gameObject.name.Equals(colliders[2]);

        if (upgrades || bullets || bandages)
        {
            standings[(upgrades ? 0 : bullets ? 1 : 2)] = false;
            warnText.text = "";
        }

    }

    private void Upgrade()
    {
        shoot.UpgradeLevel();
        upgradeLevel++;
        UpdateText();
        levelText.text = "Level " + upgradeLevel;
    }

    private void BuyBullets()
    {
        shoot.BuyMagazine();
    }

    private void BuyPotions()
    {
        playerPotion.AddPotion();
    }

    void UpdateText()
    {
        if(standings[0])
            warnText.text = "Press E To Upgrade Your Weapon ($" + upgradePrice * shoot.GetUpgradeLevel() + ")";
        else if(standings[1])
        {
            warnText.text = "Press E To Buy Ammo ($" + bulletPrice + ")";
        }
        else
        {
            warnText.text = "Press E To Buy Potions ($" + bandagePrice + ")";
        }
    }

}