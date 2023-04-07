using UnityEngine;
using TMPro;

public class PlayerCollectables : MonoBehaviour
{

    public TMP_Text balanceText;

    private int playerBalance = 0;

    public AudioSource src;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("z_coin"))
        {
            src.Play();   
            Destroy(col.transform.parent.gameObject);
            GiveBalance(Random.Range(50, 100));
        }
    }

    private void GiveBalance(int amount)
    {
        playerBalance += amount;
        balanceText.text = playerBalance.ToString();
    }

    public int GetBalance()
    {
        return playerBalance;
    }

    public void TakeBalance(int amount)
    {
        playerBalance -= amount;
        if(playerBalance < 0) playerBalance = 0;
        balanceText.text = playerBalance.ToString();
    }
}
