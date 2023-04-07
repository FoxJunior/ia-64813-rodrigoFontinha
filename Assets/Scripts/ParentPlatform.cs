using UnityEngine;

public class ParentPlatform : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
            col.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
            col.transform.SetParent(null);
    }

}
