using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRanged : EnemyAI
{
    [SerializeField]
    private GameObject projectile;

    private bool hasAttackedPlayer;


    private void Start()
    {
        hasAttackedPlayer = false;
        SpawnProjectile();
    }

    public override void AttackPlayer()
    {
        //Retain player positon for when they go out of range
        walkPoint = player.position;
        walkPointSet = true;

        transform.LookAt(player);

        if (hasAttackedPlayer != true)
        {
            SpawnProjectile();

            hasAttackedPlayer = true;

            StartCoroutine(ResetAttack());
        }

        agent.SetDestination(transform.position);
    }

    private void SpawnProjectile()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(Random.Range(10, 20));
        hasAttackedPlayer = false;
    }
}
