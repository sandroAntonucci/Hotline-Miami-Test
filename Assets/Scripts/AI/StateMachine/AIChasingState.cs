using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasingState : AIBaseState
{
    NavMeshAgent agent;

    public override void EnterState(AIHandler handler)
    {
        agent = handler.GetComponent<NavMeshAgent>();
        if (handler.enemyType == EnemyType.Melee)
            agent.stoppingDistance = handler.meleeEnemyDistance;
        else if (handler.enemyType == EnemyType.Ranged)
            agent.stoppingDistance = handler.rangedEnemyDistance;
    }

    public override void UpdateState(AIHandler handler)
    {
        bool isInRange = handler.IsInAttackRange();
        bool isInVision = handler.IsPlayerInVision();
        GameObject playerObject = handler.GetPlayerObject();

        if (!isInRange)
        {
            agent.destination = playerObject.transform.position;
        }

        Vector3 directionTowardsPlayer = (playerObject.transform.position - handler.transform.position).normalized;
        handler.RotateTowardsPlayer(directionTowardsPlayer);

        if (isInRange && !isInVision)
            agent.stoppingDistance--;

        if (isInRange && isInVision)
        {
            handler.ChangeState(handler.attackingState);
        }  
    }

    public override void ExitState(AIHandler handler)
    {

    }
}
