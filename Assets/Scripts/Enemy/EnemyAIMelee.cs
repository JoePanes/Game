using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMelee : EnemyAI
{
    public override void AttackPlayer()
    {
        walkPoint = player.position;

        attackingPlayer = true;
        if (canSprint)
        {
            agent.speed = sprintSpeed;
        }

        agent.SetDestination(walkPoint);
    }

}
