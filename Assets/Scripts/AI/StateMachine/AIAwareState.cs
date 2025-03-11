using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {

    }

    public override void UpdateState(AIHandler handler)
    {
        bool playerInVision = handler.IsPlayerInVision();
        if (playerInVision)
        {
            if (handler.IsInAttackRange())
                handler.ChangeState(handler.attackingState);
            else
                handler.ChangeState(handler.chasingState);
        } else
        {
            handler.ChangeState(handler.chasingState);
        }
    }

    public override void ExitState(AIHandler handler)
    {

    }
}
