using UnityEngine;

public class CloseSafeZoneDoor : MonoBehaviour
{

    private Animator anim;
    private SpawnFirstStage sfl;

    void Start()
    {
        anim = transform.parent.Find("Pivot").GetComponent<Animator>();
        sfl = transform.parent.Find("PresurePlateIn").GetComponent<SpawnFirstStage>();
    }

    void CloseDoor()
    {
        
        anim.ResetTrigger("Open");
        anim.SetTrigger("Close");
        sfl.PutCDAndRmStatus();
        sfl.InvokeReset();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sfl.GetStatus() && !sfl.GetCD())
            {
                CloseDoor();
            }
        }
    }

}
