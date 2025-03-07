using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState
{
    public abstract void EnterState(AIHandler handler);

    public abstract void UpdateState(AIHandler handler);

    public abstract void ExitState(AIHandler handler);
}
