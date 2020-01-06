using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;

    public Color baseColor = Color.white;
    public Color wallColor = Color.black;

    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.Log("GraphView: No graph to initialize!");
            return;
        }

        nodeViews = new NodeView[graph.Width, graph.Height];

        foreach (Node node in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            
            NodeView nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                nodeView.Init(node);
                nodeViews[node.xIndex, node.yIndex] = nodeView;

                if (node.nodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(wallColor);
                }
                else
                {
                    nodeView.ColorNode(baseColor);
                }
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }

    public void ShowNodeArrows(Node node)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

            if (nodeView != null)
            {
                nodeView.ShowArrow();
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes)
    {
        foreach (Node node in nodes)    
        {
            ShowNodeArrows(node);
        }
    }
}
