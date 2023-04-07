using UnityEngine;
using TMPro;

public class SpawnSecondStage : MonoBehaviour
{

    private TMP_Text guideText;
    private bool doorStatus;
    public GameObject secondStage;

    void Start()
    {
        guideText = GameObject.Find("QuestText").GetComponent<TMP_Text>();
        doorStatus = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!doorStatus && guideText.text.Contains("Enter Second Stage") && other.CompareTag("Player"))
        {
            doorStatus = true;
            secondStage.SetActive(true);
            transform.parent.Find("Pivot").GetComponent<Animator>().SetTrigger("Open");
            guideText.text = "Choose a Path To The Final Stage";
        }
    }
}