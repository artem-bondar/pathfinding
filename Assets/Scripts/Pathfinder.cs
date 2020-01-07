using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Node startNode;
    private Node goalNode;

    private Graph graph;
    private GraphView graphView;

    private Queue<Node> frontierNodes;

    private List<Node> exploredNodes;
    private List<Node> pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    public bool isComplete = false;

    private int iterations = 0;

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("Pathfinder: Init error: missing component(s)!");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("Pathfinder: Init error: start and goal nodes must be unblocked!");
            return;
        }

        this.graph = graph;
        this.graphView = graphView;
        this.startNode = start;
        this.goalNode = goal;

        ShowColors(graphView, start, goal);

        frontierNodes = new Queue<Node>();
        frontierNodes.Enqueue(start);

        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();

        for (int x = 0; x < graph.Width; x++)
        {
            for (int y = 0; y < graph.Height; y++)
            {
                graph.nodes[x, y].Reset();
            }
        }

        isComplete = false;
        iterations = 0;
    }

    private void ShowColors() => ShowColors(graphView, startNode, goalNode);

    private void ShowColors(GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            return;
        }

        if (frontierNodes != null)
        {
            graphView.ColorNodes(frontierNodes.ToList(), frontierColor);
        }

        if (exploredNodes != null)
        {
            graphView.ColorNodes(exploredNodes, exploredColor);
        }

        if (pathNodes != null && pathNodes.Count > 0)
        {
            graphView.ColorNodes(pathNodes, pathColor);
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    private void ExpandFrontier(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!exploredNodes.Contains(node.neighbours[i]) &&
                    !frontierNodes.Contains(node.neighbours[i]))
                {
                    node.neighbours[i].previous = node;
                    frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null)
        {
            return path;
        }

        path.Add(endNode);

        Node currentNode = endNode.previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        yield return null;

        while (!isComplete)
        {
            if (frontierNodes.Count > 0)
            {
                Node currentNode = frontierNodes.Dequeue();
                iterations++;

                if (!exploredNodes.Contains(currentNode))
                {
                    exploredNodes.Add(currentNode);
                }

                ExpandFrontier(currentNode);

                if (frontierNodes.Contains(goalNode))
                {
                    pathNodes = GetPathNodes(goalNode);
                }

                ShowColors();
                
                if (graphView != null)
                {
                    graphView.ShowNodeArrows(frontierNodes.ToList(), arrowColor);

                    if (frontierNodes.Contains(goalNode))
                    {
                        graphView.ShowNodeArrows(pathNodes, highlightColor);
                    }
                }

                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                isComplete = true;
            }
        }
    }
}
