using UnityEngine;

public class WeaponHolder : MonoBehaviour
{

    private readonly int damage = 5;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAi>().GetHit(damage);
        }
    }

}