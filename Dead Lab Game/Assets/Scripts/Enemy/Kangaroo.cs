﻿using System.Collections;
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
        prevKangarooPosition = transform.position;
    }

    private float walkRadius = 20f;
    private Vector3 finalPosition;
    private Vector3 prevKangarooPosition;

    private int count;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();

        bool isSeePlayer = eyes.IsSeePlayer();
        if (playerSee && !invisibled && reloaded && isSeePlayer)
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
                reloaded = false;
            }
        }

        if (!reloaded && !invisibled)
        {
            if (playerSee && (currentCoolDownTime == 0 || transform.position == prevKangarooPosition) && HasPath())
            {
                navMeshAgent.speed = 10f;
                GoToRandomPosition();
            }
            prevKangarooPosition = transform.position;

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
                    if (transform.position != prevKangarooPosition)
                    {
                        RunForPlayer();
                        prevKangarooPosition = transform.position;
                    }
                    else
                    {
                        Patrol();
                    }

                }
            }
            else
            {

                if (lastPlayerPosition != Vector3.zero && transform.position != prevKangarooPosition)
                {
                    CheckLastPlayerPosition();
                    prevKangarooPosition = transform.position;
                }
                else
                {
                    Patrol();
                }
            }
        }
    }

    public bool playerSee { get; set; }
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
        if (HasPath())
        {
            animator.SetBool("Walking", true);
            navMeshAgent.SetDestination(player.transform.position);
            transform.LookAt(player.transform.position);
            lastPlayerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        }
        else
        {
            Patrol();
        }
    }

    private bool HasPath()
    {
        NavMeshPath path = new NavMeshPath();
        return navMeshAgent.CalculatePath(player.transform.position, path);
    }


    private void CheckLastPlayerPosition()
    {
        //Debug.Log(navMeshAgent.);
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
        //Debug.Log(navMeshAgent.pathStatus);

    }

    private void GoToRandomPosition()
    {
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            finalPosition = hit.position;
            animator.SetBool("Walking", true);
            navMeshAgent.SetDestination(finalPosition);
        } while (Vector3.Distance(finalPosition, player.transform.position) <= 5);


    }

    private void Attack()
    {
        transform.LookAt(player.transform.position);
        animator.SetTrigger("Attacking");
    }

    private void Patrol()
    {
        navMeshAgent.speed = 2f;
        if (transform.position == prevKangarooPosition)
        {
            GoToRandomPosition();
        }
        prevKangarooPosition = transform.position;
    }

    private void RunAway()
    {
        navMeshAgent.speed = 4f;
        if (transform.position == prevKangarooPosition)
        {
            GoToRandomPosition();
        }
        prevKangarooPosition = transform.position;
    }
}
