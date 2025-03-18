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
        BaseGun gunComponent = handler.gameObject.transform.GetChild(1).GetComponent<BaseGun>();
        if (gunComponent && gunComponent.canShoot && gunComponent.currentAmmo > 0)
        {
            gunComponent.Shoot();
        }
        handler.ChangeState(handler.chasingState);
    }

    public override void ExitState(AIHandler handler)
    {

    }
}