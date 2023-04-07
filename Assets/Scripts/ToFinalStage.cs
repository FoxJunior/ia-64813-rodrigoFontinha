using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToFinalStage : MonoBehaviour
{
    public GameObject Zombie;
    private TMP_Text guideText;
    private bool doorStatus, bossStarted;
    public Slider slider;
    private int bossHealth;

    void Start()
    {
        bossHealth = Random.Range(3500, 4500);

        guideText = GameObject.Find("QuestText").GetComponent<TMP_Text>();
        doorStatus = bossStarted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!doorStatus && guideText.text.Contains("The Final Stage") && other.CompareTag("Player"))
        {
            doorStatus = true;
            transform.parent.Find("Pivot").GetComponent<Animator>().SetTrigger("Open");
            guideText.text = "Kill The Final Boss";
        }
    }

    public void SpawnBoss()
    {

        if (!bossStarted)
        {

            GameObject stage = GameObject.Find("Stage");
            bossStarted = true;

            for (int i = 1; i < 5; i++)
                stage.transform.GetChild(i).gameObject.SetActive(true);

            stage.transform.GetChild(7).gameObject.SetActive(false);
            GameObject z = Instantiate(Zombie, stage.transform.GetChild(6).position, Quaternion.identity);
            z.GetComponent<ObjectHealth>().ChangeMaxHealth(bossHealth);

            slider.gameObject.SetActive(true);
            slider.maxValue = bossHealth;
            slider.value = bossHealth;

        }

    }

}