//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    patrol,
    chase,
    attack
}
public class EnemyController : MonoBehaviour
{
    private EnemyStats enemyStats;

    private NavMeshAgent navMeshAgent;

    private EnemyState enemyState;

    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;

    [SerializeField] private float chaseDistance;
    [SerializeField] private float currentChaseDistance;

    [SerializeField] private float attackDistance;
    [SerializeField] private float chaseAfterAttackDistance;

    [SerializeField] private float patrolRadiusMax;
    [SerializeField] private float patrolRadiusMin;

    [SerializeField] private float patrolBasicTime;
    private float patrolTimer;

    [SerializeField] private float attackReload;
    private float attackTimer;

    public Transform target;


    private void Awake()
    {
        enemyStats = transform.GetChild(0).GetComponent<EnemyStats>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
        enemyState = EnemyState.patrol;

        patrolTimer = patrolBasicTime;
        attackTimer = attackReload;

        currentChaseDistance = chaseDistance;
    }
    void Update()
    {
        CheckDistanceToPlayer();

        Patrol();
        Chase();
        Attack();
    }

    public void ChangeChaseDistance(float newDistance)
    {
        currentChaseDistance = newDistance; 
    }
    private void CheckDistanceToPlayer()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > currentChaseDistance)
        {
            enemyState = EnemyState.patrol;
        }
        else if(distance > attackDistance && distance <= currentChaseDistance)
        {
            enemyState = EnemyState.chase;
        }
        else if(distance <= attackDistance)
        {
            enemyState = EnemyState.attack;

            currentChaseDistance = chaseDistance;
        }
    }
    private void Patrol()
    {
        if(enemyState == EnemyState.patrol)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = walkSpeed;

            if(CheckPatrolTimer())
            {
                SetNewRandomDestination();
            }
        }
    }
    private void SetNewRandomDestination()
    {
        float randomRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 randomDirection = transform.position + Random.insideUnitSphere * randomRadius;

        NavMeshHit meshHit;

        NavMesh.SamplePosition(randomDirection, out meshHit, randomRadius, -1);

        navMeshAgent.SetDestination(meshHit.position);
    }
    private bool CheckPatrolTimer()
    {
        patrolTimer += Time.deltaTime;

        if(patrolTimer > patrolBasicTime)
        {
            patrolTimer = 0;

            return true;
        }

        return false;
    }
    private bool CheckAttackTimer()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer > attackReload)
        {
            attackTimer = 0;

            return true;
        }

        return false;
    }


    private void Chase()
    {
        if (enemyState == EnemyState.chase)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = runSpeed;

            navMeshAgent.SetDestination(target.position);
        }
    }
    private void Attack()
    {
        if (enemyState == EnemyState.attack)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;

            if(CheckAttackTimer())
            {
                target.GetComponent<PlayerStats>().TakeDamage(enemyStats.GetAttackPower());
            }
        }
    }
}
