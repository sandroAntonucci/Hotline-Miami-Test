using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AIHandler : MonoBehaviour
{

    public float fieldOfView = 30.0f;
    public float distanceOfView = 10;
    public EnemyType enemyType;
    public float rangedEnemyDistance = 5;
    public float meleeEnemyDistance = 2;

    public AIBaseState currentAiState; 
    public AIIdleState idleState = new AIIdleState();
    public AIAwareState awareState = new AIAwareState();
    public AIChasingState chasingState = new AIChasingState();
    public AIAttackingState attackingState = new AIAttackingState();
    public AIStunnedState stunnedState = new AIStunnedState();
    public AIDeadState deadState = new AIDeadState();


    void Start()
    {
        currentAiState = idleState;
        currentAiState.EnterState(this);
    }

    void Update()
    {
        currentAiState.UpdateState(this);
    }

    public void ChangeState(AIBaseState newState)
    {
        currentAiState.ExitState(this);
        currentAiState = newState;
        newState.EnterState(this);
    }

    public void RotateTowardsPlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }

    public GameObject GetPlayerObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public bool IsPlayerInVision()
    {
        // Debug AI fov.
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * distanceOfView, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * distanceOfView, Color.blue);

        GameObject playerObject = GetPlayerObject();
        if (playerObject == null)
            return false;

        Vector3 directionTowardsPlayer = (playerObject.transform.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, directionTowardsPlayer);

        if (angle < fieldOfView / 2)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distanceToPlayer < distanceOfView)
            {
                // Debug collision check.
                Debug.DrawRay(transform.position, playerObject.transform.position, Color.red);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionTowardsPlayer, out hit, distanceOfView))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        RotateTowardsPlayer(directionTowardsPlayer);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsInAttackRange()
    {
        GameObject playerObject = GetPlayerObject();
        if (playerObject == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        if (enemyType == EnemyType.Melee && distanceToPlayer < meleeEnemyDistance)
            return true;
        else if (enemyType == EnemyType.Ranged && distanceToPlayer < rangedEnemyDistance)
            return true;
        
        return false;
    }
}
