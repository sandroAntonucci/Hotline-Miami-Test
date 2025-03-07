using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIBaseState
{

    public override void EnterState(AIHandler handler)
    {

    }

    public override void UpdateState(AIHandler handler)
    {
        bool playerInVision = handler.IsPlayerInVision();
        if (playerInVision)
        {
            handler.ChangeState(handler.awareState);
        }
    }

    public override void ExitState(AIHandler handler)
    {

    }
}
