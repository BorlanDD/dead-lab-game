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

    private AudioManager audioManager;

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
        audioManager = AudioManager.GetInstance();
        player = Player.GetInstance();
        prevKangarooPosition = transform.position;
    }

    private float walkRadius = 20f;
    private Vector3 finalPosition;
    private Vector3 prevKangarooPosition;

    private int count;
    private bool afterChase;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();

        bool isSeePlayer = eyes.IsSeePlayer();
        if (playerSee && !invisibled && reloaded && (isSeePlayer || shooted))
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
            currentCoolDownTime += Time.deltaTime;
            if (currentCoolDownTime >= coolDown)
            {
                reloaded = true;
                currentCoolDownTime = 0;
            }
        }



        if (isSeePlayer || shooted)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= navMeshAgent.stoppingDistance)
            {
                Attack();
                audioManager.PlayBattleLoopSound();
                afterChase = true;
            }
            else
            {
                if (transform.position != prevKangarooPosition && HasPath())
                {
                    RunForPlayer();
                    audioManager.PlayBattleLoopSound();
                    afterChase = true;
                }
                else
                {
                    Patrol();
                    if (afterChase)
                    {
                        audioManager.PlayBattleEndSound();
                        afterChase = false;
                    }
                }

            }
        }
        else
        {

            if (lastPlayerPosition != Vector3.zero && transform.position != prevKangarooPosition && HasPath())
            {
                CheckLastPlayerPosition();
                audioManager.PlayBattleLoopSound();
                afterChase = true;
            }
            else
            {
                Patrol();
                lastPlayerPosition = Vector3.zero;
                if (afterChase)
                {
                    audioManager.PlayBattleEndSound();
                    afterChase = false;
                }
            }
        }

        prevKangarooPosition = transform.position;
        shooted = false;
    }

    public override void Die()
    {
        base.Die();
        audioManager.PlayBattleEndSound();
        afterChase = false;
    }

    public bool playerSee { get; set; }
    private bool invisibled;
    private float currentInvisibleTime;

    [SerializeField]
    private float invisibleTime = 6f;


    private float currentCoolDownTime;
    [SerializeField]
    private float coolDown = 10f;
    [SerializeField]
    private float runningSpeed = 3.75f;
    [SerializeField]
    private float normalSpeed = 2.5f;

    private bool reloaded = true;

    private float current = 0f;
    private float delay = 1f;

    public bool shooted { get; set; }
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

    private Vector3 lastPlayerPosition;
    private void RunForPlayer()
    {
        navMeshAgent.speed = runningSpeed;
        animator.SetBool("Walking", true);
        navMeshAgent.SetDestination(player.transform.position);
        transform.LookAt(player.transform.position);
        lastPlayerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
    }

    private bool HasPath()
    {
        NavMeshPath path = new NavMeshPath();
        return navMeshAgent.CalculatePath(player.transform.position, path);
    }

    private void CheckLastPlayerPosition()
    {
        navMeshAgent.speed = runningSpeed;
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

    private void GoToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        finalPosition = hit.position;
        animator.SetBool("Walking", true);
        navMeshAgent.SetDestination(finalPosition);
    }

    private void Attack()
    {
        transform.LookAt(player.transform.position);
        animator.SetTrigger("Attacking");
        MakeDamage(damage);
    }

    private void MakeDamage(float damage)
    {
        player.GetDamage(damage * Time.deltaTime);
    }


    private void Patrol()
    {
        navMeshAgent.speed = normalSpeed;
        if (transform.position == prevKangarooPosition)
        {
            GoToRandomPosition();
        }
        prevKangarooPosition = transform.position;
    }
}
