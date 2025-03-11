using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {
        handler.ChangeState(handler.chasingState);
    }

    public override void UpdateState(AIHandler handler)
    {

    }

    public override void ExitState(AIHandler handler)
    {

    }
}
