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
        if (!isInRange && !isInVision)
            agent.destination = handler.GetPlayerObject().transform.position;
        else if (isInRange && !isInVision)
            agent.destination = handler.GetPlayerObject().transform.position;
        else
            handler.ChangeState(handler.attackingState);
    }

    public override void ExitState(AIHandler handler)
    {

    }
}
