using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {
        // gotta remove that nasty AI from the region agents :>
        handler.currentNodeRegion.agentsInRegion.Remove(handler);
        handler.DestroyObject(handler.gameObject);
    }

    public override void UpdateState(AIHandler handler)
    {

    }

    public override void ExitState(AIHandler handler)
    {

    }
}
