using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{

    private NavMeshAgent agent;

    private Animator anim;

    private Transform player;

    public GameObject coin;

    private ObjectHealth objectHealthP, objectHealthE;

    public LayerMask whatIsGround, whatIsPlayer;

    private TMP_Text guideText;

    public ParticleSystem hitParticle;

    //Patroling
    private Vector3 walkPoint, gunShot;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    private bool gotHit, alive, playerInsightRange, playerInAttackRange;

    private readonly int damage = 10;

    private Vector3 scaleChange;

    private bool isCoroutineExecuting;

    public AudioSource src;

    public GameObject bossDropsPlane;

    private void Awake()
    {
        GameObject aux = GameObject.Find("Player");
        agent = transform.parent.GetComponent<NavMeshAgent>();
        anim = transform.parent.GetComponent<Animator>();
        objectHealthE = transform.parent.GetComponent<ObjectHealth>();
        objectHealthP = aux.GetComponent<ObjectHealth>();
        guideText = GameObject.Find("QuestText").GetComponent<TMP_Text>();
        player = aux.transform;
        alreadyAttacked = gotHit = isCoroutineExecuting = false;
        alive = true;
        scaleChange = new Vector3(0.001f, 0.001f, 0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            playerInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (gotHit && Vector3.Distance(transform.position, gunShot) < 2.3f) gotHit = false;

            if (!playerInsightRange && !playerInAttackRange && !gotHit) Patroling();
            if (playerInsightRange && !playerInAttackRange) ChasePlayer();
            if (playerInsightRange && playerInAttackRange) AttackPlayer();
        }
        else
        {
            if(transform.localScale.x > 0.0f)
                transform.localScale -= scaleChange;
        }

    }

    private void Patroling()
    {
        agent.speed = 1;

        anim.ResetTrigger("attacking");
        anim.ResetTrigger("chasing");
        anim.SetTrigger("walking");

        if (!walkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.parent.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

        if (objectHealthE.GetHealth() < objectHealthE.GetMaxHealth())
        {
            StartCoroutine(EnemyRestoreHealth(1f));
        }

    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.parent.position.x + randomX, transform.parent.position.y, transform.parent.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.parent.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {

        anim.ResetTrigger("attacking");
        anim.ResetTrigger("walking");
        anim.SetTrigger("chasing");

        if (!transform.parent.gameObject.name.Contains("Boss"))
            agent.speed = 3;
        else
            agent.speed = 5;

        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

        Vector3 targetPosition = new(player.position.x,
                                        transform.parent.position.y,
                                        player.position.z);

        transform.parent.LookAt(targetPosition);

        if (!alreadyAttacked)
        {
            anim.ResetTrigger("chasing");
            anim.SetTrigger("attacking");

            src.Play();

            if (!transform.parent.gameObject.name.Contains("Boss"))
                objectHealthP.TakeDamage(damage);
            else
                objectHealthP.TakeDamage(damage * 2);

            if (objectHealthP.GetHealth() == 0)
            {
                GameObject.Find("MainCanvas").GetComponent<PauseMenu>().PauseGame(false);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        else
        {
            anim.SetTrigger("attackCD");
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    IEnumerator EnemyRestoreHealth(float time)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(time);

        objectHealthE.GiveHealth(1);

        isCoroutineExecuting = false;
    }

    private void DestroyEnemy()
    {
        Destroy(transform.parent.gameObject);
        if (!transform.parent.gameObject.name.Contains("Boss"))
        {
            Vector3 aux = new(0, 0.6f, 0f);
            GameObject coinO = Instantiate(coin, transform.parent.position += aux, coin.transform.rotation);
            Destroy(coinO, 60);
            UpdateGuideText();
        }

    }

    private void UpdateGuideText()
    {

        if (guideText.text.Contains("Zombies"))
        {
            string aux = guideText.text.Split("\n")[1];
            int numDead = int.Parse(aux.Split("/")[0].Trim()) + 1;
            int totalNum = int.Parse(aux.Split("/")[1].Trim());
            if (numDead == totalNum)
            {
                guideText.text = "Upgrade Yourself (Shop)\n Enter Second Stage";
                GameObject.Find("ToFirstStage").transform.GetChild(3).gameObject.SetActive(false);
                SpawnFirstStage firstStage = GameObject.Find("ToFirstStage").transform.GetChild(2).GetComponent<SpawnFirstStage>();
                if(!firstStage.GetStatus())
                    firstStage.OpenDoor();
            }
            else
            {
                guideText.text = "Kill Zombies\n " + numDead + " / " + totalNum;
            }
        }
    }

    public void GetHit(int dmg)
    {

        objectHealthE.TakeDamage(dmg);
        hitParticle.Play();

        int enemyHealth = objectHealthE.GetHealth();

        if (enemyHealth <= 0)
        {
            alive = false;
            agent.isStopped = true;
            anim.SetTrigger("death");
            Invoke(nameof(DestroyEnemy), 2.0f);
        }else if (!playerInsightRange)
        {
            gotHit = true;
            gunShot = player.position;
            agent.SetDestination(player.position);
            agent.speed = 3;
            anim.ResetTrigger("walking");
            anim.SetTrigger("chasing");
        }

        // Boss
        if (transform.parent.gameObject.name.Contains("Boss"))
        {
            Slider slider = GameObject.Find("BossHealthBar").GetComponent<Slider>();
            slider.value = enemyHealth;
            if (enemyHealth <= 0)
            {
                slider.gameObject.SetActive(false);
                Transform Stage = GameObject.Find("Stage").transform;
                for (int i = 1; i < 4; i++)
                {
                    Stage.GetChild(i).gameObject.SetActive(false);
                }
                GameObject SpotEscape = Stage.GetChild(8).gameObject;
                GameObject Plane = Instantiate(bossDropsPlane, SpotEscape.transform.position, Quaternion.identity);
                Plane.transform.SetParent(SpotEscape.transform);
                SpotEscape.SetActive(true);
                guideText.text = "Go To The Plane And Escape";
            }
            int spawnZ = Random.Range(0, 10);
            if(spawnZ < 2)
            {
                GameObject Zombie = GameObject.Find("ToFirstStage").transform.GetChild(2).GetComponent<SpawnFirstStage>().GetZombie();
                GameObject z = Instantiate(Zombie, transform.parent.position, Quaternion.identity);
                z.transform.GetChild(0).GetComponent<EnemyAi>().sightRange = 100;

            }
        }

    }

}
