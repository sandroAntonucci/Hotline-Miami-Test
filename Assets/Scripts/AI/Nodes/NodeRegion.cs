using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRegion : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();
    public List<AIHandler> agentsInRegion = new List<AIHandler>();

    void Start()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go)
            { 
                Node node = go.GetComponent<Node>();
                if (node)
                {
                    node.region = this;
                    nodes.Add(node);
                }
            }
        }
    }

    public Node GetRandomNode()
    {
        return nodes[Random.Range(0, nodes.Count)];
    }

    public float GetNodeRegionCongestionRate()
    {
        return (float)agentsInRegion.Count / nodes.Count;
    }
}
