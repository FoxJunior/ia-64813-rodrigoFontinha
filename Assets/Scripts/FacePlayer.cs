using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    private Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        Vector3 targetPosition = new(player.position.x,
                                transform.position.y,
                                player.position.z);

        transform.LookAt(targetPosition);
    }
}
