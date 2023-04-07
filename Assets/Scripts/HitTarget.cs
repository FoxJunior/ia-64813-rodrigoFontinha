using UnityEngine;

public class HitTarget : MonoBehaviour
{

    private readonly int damage = 25;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            int upgradeLevel = player.GetComponent<Shoot>().GetUpgradeLevel();
            other.transform.GetComponent<EnemyAi>().GetHit(upgradeLevel * damage);
            Destroy(gameObject, 0.5f);
        }

    }

    void OnCollisionEnter(Collision other)
    {

        Destroy(gameObject);
    }

}
