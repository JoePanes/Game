using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Based on the video tutorial from https://www.youtube.com/watch?v=UjkSFoLxesw
/// </summary>
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform player;

    private Enemy enemy;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool attackingPlayer;
    public bool canSprint;

    private float normalSpeed;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        enemy = GetComponent<Enemy>();

        attackingPlayer = false;

        // give a 9% chance that the enemy is able to run faster at the player
        if (Random.Range(0, 101) > 90) canSprint = true;
        normalSpeed = agent.speed;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (enemy.CheckIfDead() == false)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();

            if (playerInSightRange && !playerInAttackRange) ChasePlayer();

            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        } else
        {
            agent.velocity = Vector3.forward * 0;
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Make sure point in on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

        //If player gets away and the enemy has been in attacking range, reset speed
        if (attackingPlayer)
        {
            attackingPlayer = false;
            agent.speed = normalSpeed;
        }
    }

    private void ChasePlayer()
    {


        walkPoint = player.position;

        agent.SetDestination(walkPoint);

        //In the event that the player runs out of range,
        //go to their last known position

        walkPointSet = true;
    }

    private void AttackPlayer()
    {
        walkPoint = player.position;

        walkPoint = player.position;

        

        attackingPlayer = true;
        if (canSprint)
        {
            agent.speed = agent.speed * 1.5f;
        }

        agent.SetDestination(walkPoint);
    }
}
