using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;



public class AIIdleState : AIBaseState
{
    private bool switchNodeDebounce = false;
    private int switchNodeWaitTime = 5;
    private int switchRegionWaitTime = 10;
    private float switchRegionNodeCongestionMax = .45f; // Si el número de la congestión supera este número, entonces no cambia.
    private int switchRegionChance = 5;

    private Coroutine DelayNextNodeCoroutine;

    public override void EnterState(AIHandler handler)
    {
        if (handler.currentNodeRegion)
        {
            handler.currentNodeRegion.agentsInRegion.Add(handler);
        }
        DelayNextNodeCoroutine = handler.StartCoroutine(SwitchRegionCoroutine(handler));
    }

    public override void UpdateState(AIHandler handler)
    {
        bool playerInVision = handler.IsPlayerInVision();
        if (playerInVision)
        {
            handler.ChangeState(handler.awareState);
        }
        else if (handler.currentNodeRegion != null && !switchNodeDebounce)
        {
            Node nextNode = handler.currentNodeRegion.GetRandomNode();
            if (nextNode != null && !nextNode.isOccupied)
            {
                NavMeshAgent aiAgent = handler.GetComponent<NavMeshAgent>();
                if (aiAgent != null && aiAgent.remainingDistance == 0 && !aiAgent.pathPending)
                {
                    aiAgent.destination = nextNode.transform.position;
                    nextNode.isOccupied = true;
                    handler.currentNode = nextNode;
                    handler.StartCoroutine(DelayNextNode(aiAgent, handler));
                }
            }
        }

        
    }

    public override void ExitState(AIHandler handler)
    {
        handler.StopCoroutine(DelayNextNodeCoroutine);
    }

    public NodeRegion GetRandomNodeRegion(AIHandler handler)
    {
        List<GameObject> gameObjects = GameObject.FindGameObjectsWithTag("NodeRegion").ToList();
        NodeRegion reg = gameObjects[Random.Range(0, gameObjects.Count)].GetComponent<NodeRegion>();
        while (reg == handler.currentNodeRegion)
        {
            reg = gameObjects[Random.Range(0, gameObjects.Count)].GetComponent<NodeRegion>();
        }
        return reg;
    }

    public IEnumerator DelayNextNode(NavMeshAgent aiAgent, AIHandler handler)
    {
        // Exit if aiAgent is intentionally null or handler is missing
        if (aiAgent == null || handler == null)
        {
            Debug.LogWarning("AI Agent or Handler is null. Exiting DelayNextNode.");
            yield break;
        }

        // Check if agent is active and placed on the NavMesh before continuing
        if (!aiAgent.isActiveAndEnabled || !aiAgent.isOnNavMesh)
        {
            Debug.LogWarning("AI Agent is not active or not placed on a NavMesh.");
            yield break;
        }

        switchNodeDebounce = true;

        // Wait for agent to reach destination, but only if it's valid
        while (aiAgent != null && aiAgent.isOnNavMesh && aiAgent.pathPending ||
               (aiAgent.isOnNavMesh && aiAgent.remainingDistance > aiAgent.stoppingDistance))
        {
            yield return new WaitForSeconds(0.1f);

            // If the agent becomes null while waiting, exit gracefully
            if (aiAgent == null)
            {
                Debug.LogWarning("AI Agent became null while waiting.");
                yield break;
            }
        }

        // Double-check aiAgent before accessing properties
        if (aiAgent == null || handler == null || handler.currentNode == null)
        {
            Debug.LogWarning("AI Agent or Handler became null before finishing.");
            yield break;
        }

        // Rotate the AI to face its destination
        handler.transform.rotation = handler.currentNode.transform.rotation;

        yield return new WaitForSeconds(Random.Range(switchNodeWaitTime, switchNodeWaitTime * 2));

        if (handler.currentNode != null)
        {
            handler.currentNode.isOccupied = false;
            handler.currentNode = null;
        }

        switchNodeDebounce = false;
    }


    public IEnumerator SwitchRegionCoroutine(AIHandler handler)
    {
        while (true)
        {
            // Chance to switch region!
            int num = Random.Range(0, 100);
            if (num < switchRegionChance)
            {
                NodeRegion region = GetRandomNodeRegion(handler);
                float regionCongestion = region.GetNodeRegionCongestionRate();
                if (regionCongestion < switchRegionNodeCongestionMax) 
                {
                    handler.currentNodeRegion.agentsInRegion.Remove(handler);
                    handler.currentNodeRegion = region;
                    handler.currentNodeRegion.agentsInRegion.Add(handler);
            }
                }
            yield return new WaitForSeconds(switchRegionWaitTime);
        }
    }
}
