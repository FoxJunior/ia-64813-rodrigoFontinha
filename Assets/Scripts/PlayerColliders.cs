using System.Collections;
using UnityEngine;

public class PlayerColliders : MonoBehaviour
{

    private ObjectHealth objectHealthP;
    private PlayerMovement playerMovement;
    private readonly int blueLavaDmg = 2, blueLavaSlow = 20, movingObjectSlow = 8;
    private readonly int redLavaDmg = 5, healingDmg = 1;
    private bool isCoroutineExecuting, standing, healing;
    private float oGroundDrag;

    void Start()
    {
        objectHealthP = gameObject.GetComponent<ObjectHealth>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        oGroundDrag = playerMovement.groundDrag;
        standing = healing = false;
    }

    IEnumerator HazardTakeDamage(float time, int dmg)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(time);

        if (standing)
        {
            objectHealthP.TakeDamage(dmg);
            IfAlive();
        }  
        else if(healing)
            objectHealthP.GiveHealth(dmg);

        isCoroutineExecuting = false;
    }

    void TeleportPlayer()
    {
        transform.position = GameObject.Find("TeleportIfLava").transform.position;
    }

    bool IfAlive()
    {
        if (objectHealthP.GetHealth() == 0)
        {
            GameObject.Find("MainCanvas").GetComponent<PauseMenu>().PauseGame(false);
            return false;
        }
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("moving_object"))
        {
            playerMovement.groundDrag = movingObjectSlow;
            transform.SetParent(other.transform.parent);
        }else if (other.name.Equals("CallElevatorDown") || other.name.Equals("CallElevatorUp"))
        {
            Elevator elev = GameObject.Find("Platform").GetComponent<Elevator>();
            bool state = elev.GetState();
            if (other.name.Equals("CallElevatorDown") && state || (other.name.Equals("CallElevatorUp") && !state))
            {
                elev.Move();
            }
        }else if (other.name.Equals("TriggerBoss"))
        {
            GameObject.Find("ToLastStage").transform.GetChild(2).GetComponent<ToFinalStage>().SpawnBoss();
        }else if (other.name.Contains("biplane"))
        {

            GameObject.Find("MainCanvas").GetComponent<PauseMenu>().PauseGame(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        bool blueLava = other.CompareTag("blue_lava");
        bool redLava = other.CompareTag("red_lava");
        if (blueLava || redLava)
        {
            playerMovement.groundDrag = (blueLava ? blueLavaSlow : oGroundDrag);
            standing = true;
            
            if (redLava)
            {
                objectHealthP.TakeDamage(redLavaDmg);
                if(IfAlive())
                    TeleportPlayer();
            }
            else
            {
                StartCoroutine(HazardTakeDamage(0.5f, blueLavaDmg));
            }
        }
        else if (other.name.Equals("Rest") && objectHealthP.GetHealth() < 100)
        {
            healing = true;
            StartCoroutine(HazardTakeDamage(5f, healingDmg));
        }
    }

    void OnTriggerExit(Collider other)
    {
        bool blueLava = other.CompareTag("blue_lava");
        bool redLava = other.CompareTag("red_lava");
        bool movingObject = other.CompareTag("moving_object");
        bool home = other.name.Equals("Rest");
        if (blueLava || redLava || movingObject)
        {
            playerMovement.groundDrag = oGroundDrag;
            if(!movingObject)
                standing = false;
            if(movingObject)
                transform.SetParent(null);
        }else if (home)
        {
            healing = false;
        }
    }
}