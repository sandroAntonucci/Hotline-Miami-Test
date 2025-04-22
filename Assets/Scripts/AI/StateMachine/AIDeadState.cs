using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIDeadState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {
        // gotta remove that nasty AI from the region agents :>
        handler.currentNodeRegion.agentsInRegion.Remove(handler);
        //handler.DestroyObject(handler.gameObject);

        // Adds force to the rigidbody

        handler.GetComponent<NavMeshAgent>().enabled = false;
        
        
        Rigidbody rb = handler.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; 
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.AddForce(-Vector3.forward * 15, ForceMode.Force);
            rb.AddTorque(Random.insideUnitSphere * 15, ForceMode.Force);
        }

        handler.weapon.DropWeapon();
        handler.weapon.SetUpPlayerWeapon();
        
    }

    public override void UpdateState(AIHandler handler)
    {

    }

    public override void ExitState(AIHandler handler)
    {

    }
}
