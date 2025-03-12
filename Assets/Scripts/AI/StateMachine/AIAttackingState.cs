using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackingState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {

    }

    public override void UpdateState(AIHandler handler)
    {
        Debug.Log("Attacking! :D");
        handler.ChangeState(handler.chasingState);
    }

    public override void ExitState(AIHandler handler)
    {

    }
}