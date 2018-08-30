using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kangaroo : Enemy
{

    public NavMeshAgent navMeshAgent;

    private MeshRenderer meshRenderer;
    public SkinnedMeshRenderer head;
    public SkinnedMeshRenderer body;

    public Material defaultMat;
    public Material trancparencyMat;


    public float lookRadius = 10f;
    private Player player;
    private float damage;

    public FieldOfView eyes;

    private Animator animator;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }

    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        animator = GetComponent<Animator>();
        health = 100;
        damage = 0.1f;
        invisibled = false;
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = Player.GetInstance();
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();

        /* timer += Time.deltaTime;
 
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        } */

        if (invisible && !invisibled && reloaded)
        {
            BecomeInvisible();
        }

        if (invisibled)
        {
            currentInvisibleTime += Time.deltaTime;
            if (currentInvisibleTime >= invisibleTime)
            {
                head.material = defaultMat;
                body.material = defaultMat;

                currentInvisibleTime = 0f;
                invisibled = false;
            }
        }

        if (!reloaded && !invisibled)
        {
            if (currentCoolDownTime == 0)
            {
                navMeshAgent.speed = 40f;
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                navMeshAgent.SetDestination(newPos);

            }

            currentCoolDownTime += Time.deltaTime;
            if (currentCoolDownTime >= coolDown)
            {
                reloaded = true;
                currentCoolDownTime = 0;
            }
        }
        else
        {
            navMeshAgent.speed = 3f;

            if (eyes.IsSeePlayer())
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= navMeshAgent.stoppingDistance)
                {
                    Attack();
                }
                else
                {
                    RunForPlayer();
                }

            }
            else
            {
                if (lastPlayerPosition != Vector3.zero)
                {
                    CheckLastPlayerPosition();
                }
                else
                {

                }
            }
        }




    }

    private bool invisible;
    private bool invisibled;
    private float currentInvisibleTime;

    [SerializeField]
    private float invisibleTime = 6f;


    private float currentCoolDownTime;
    [SerializeField]
    private float coolDown = 10f;

    private bool reloaded = true;

    private float current = 0f;
    private float delay = 1f;
    private void BecomeInvisible()
    {
        if (current >= delay)
        {
            head.material = trancparencyMat;
            body.material = trancparencyMat;
            invisibled = true;
            reloaded = false;
            current = 0;
        }
        current += Time.deltaTime;
    }



    public void SetInvisibility(bool active)
    {
        invisible = active;
    }


    public float wanderRadius = 20f;
    //public float wanderTimer;
    //private float timer;
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private Vector3 lastPlayerPosition;
    private void RunForPlayer()
    {
        animator.SetBool("Walking", true);
        navMeshAgent.SetDestination(player.transform.position);
        transform.LookAt(player.transform.position);
        lastPlayerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
    }

    private void CheckLastPlayerPosition()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), lastPlayerPosition) >= navMeshAgent.stoppingDistance)
        {
            animator.SetBool("Walking", true);
            navMeshAgent.SetDestination(lastPlayerPosition);
            transform.LookAt(navMeshAgent.nextPosition);
        }
        else
        {
            lastPlayerPosition = Vector3.zero;
            animator.SetBool("Walking", false);
            animator.SetTrigger("Looking");
        }


    }

    private void Patrol()
    {

    }

    private void Attack()
    {
        transform.LookAt(player.transform.position);
        animator.SetTrigger("Attacking");
    }


}
