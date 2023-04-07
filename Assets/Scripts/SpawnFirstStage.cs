using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnFirstStage : MonoBehaviour
{

    public GameObject Zombie;
    private List<int> whichIndex;
    private bool started = false;
    public bool doorStatus, doorCD;
    private Animator anim;
    public TMP_Text guideText;

    void Start()
    {
        doorStatus = doorCD = false;
        anim = transform.parent.Find("Pivot").GetComponent<Animator>();
    }

    void GenerateRandomIndexes(int length)
    {
        while (true)
        {
            int number = Random.Range(1, length - 1);
            if (!whichIndex.Contains(number)) whichIndex.Add(number);
            if (whichIndex.Count >= length / 2 - 1) break;
        }

        whichIndex.Add(Random.Range(0, length));
    }

    public GameObject GetZombie()
    {
        return Zombie;
    }

    void StartSpawn()
    {
        if (!started)
        {
            started = true;
            whichIndex = new List<int>();
            Transform[] allChildren = GameObject.Find("EnemiesSpawns").GetComponentsInChildren<Transform>();
            GenerateRandomIndexes(allChildren.Length - 1);

            for (int i = 0; i < whichIndex.Count; i++)
            {
                Instantiate(Zombie, allChildren[whichIndex[i]].position, Quaternion.identity);
            }
        }

        guideText.text = "Kill Zombies\n 0 / " + whichIndex.Count;

    }
    private void ResetCD()
    {
        doorCD = false;
    }

    public void PutCDAndRmStatus()
    {
        doorCD = true;
        doorStatus = false;
    }

    public void InvokeReset()
    {
        Invoke(nameof(ResetCD), 1.5f);
    }

    public bool GetStatus()
    {
        return doorStatus;
    }

    public bool GetCD()
    {
        return doorCD;
    }

    public void OpenDoor()
    {
        doorStatus = doorCD = true;
        anim.ResetTrigger("Close");
        anim.SetTrigger("Open");
        InvokeReset();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!doorStatus && !doorCD)
                OpenDoor();
            if(guideText.text.Contains("Enter First Stage"))
                StartSpawn();
        }
    }

}