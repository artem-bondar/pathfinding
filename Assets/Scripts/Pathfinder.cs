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

    private PriorityQueue<Node> frontierNodes;

    private List<Node> exploredNodes;
    private List<Node> pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

    public Color arrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    public bool isComplete = false;

    private int iterations = 0;

    public enum Mode
    {
        BreadthFirstSearch = 0,
        GreedyBestFirst = 1,
        Dijkstra = 2,
        AStar = 3
    }

    public Mode mode = Mode.BreadthFirstSearch;

    private void ShowColors(bool lerpColor = false, float lerpValue = 0.5f) =>
        ShowColors(graphView, startNode, goalNode, lerpColor, lerpValue);

    private void ShowColors(GraphView graphView, Node start, Node goal,
                            bool lerpColor = false, float lerpValue = 0.5f)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            return;
        }

        if (frontierNodes != null)
        {
            graphView.ColorNodes(frontierNodes.ToList(), frontierColor, lerpColor, lerpValue);
        }

        if (exploredNodes != null)
        {
            graphView.ColorNodes(exploredNodes, exploredColor, lerpColor, lerpValue);
        }

        if (pathNodes != null && pathNodes.Count > 0)
        {
            graphView.ColorNodes(pathNodes, pathColor, lerpColor, lerpValue * 2f);
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

    private void ExpandFrontierBreadthFirst(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!exploredNodes.Contains(node.neighbours[i]) &&
                    !frontierNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeighbor +
                        node.distanceTraveled + (int)node.nodeType;

                    node.neighbours[i].previous = node;
                    node.neighbours[i].priority = exploredNodes.Count;
                    node.neighbours[i].distanceTraveled = newDistanceTraveled;

                    frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private void ExpandFrontierGreedyBestFirst(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!exploredNodes.Contains(node.neighbours[i]) &&
                    !frontierNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeighbor +
                        node.distanceTraveled + (int)node.nodeType;

                    node.neighbours[i].previous = node;
                    node.neighbours[i].distanceTraveled = newDistanceTraveled;

                    if (graph != null)
                    {
                        node.neighbours[i].priority =
                            (int)graph.GetNodeDistance(node.neighbours[i], goalNode);
                    }

                    frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private void ExpandFrontierDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!exploredNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeighbor
                        + node.distanceTraveled + (int)node.nodeType;

                    if (float.IsPositiveInfinity(node.neighbours[i].distanceTraveled) ||
                        newDistanceTraveled < node.neighbours[i].distanceTraveled)
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].distanceTraveled = newDistanceTraveled;
                    }

                    if (!frontierNodes.Contains(node.neighbours[i]))
                    {
                        node.neighbours[i].priority = (int)node.neighbours[i].distanceTraveled;
                        frontierNodes.Enqueue(node.neighbours[i]);
                    }
                }
            }
        }
    }

    private void ExpandFrontierAStar(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!exploredNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeighbor
                        + node.distanceTraveled + (int)node.nodeType;

                    if (float.IsPositiveInfinity(node.neighbours[i].distanceTraveled) ||
                        newDistanceTraveled < node.neighbours[i].distanceTraveled)
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].distanceTraveled = newDistanceTraveled;
                    }

                    if (!frontierNodes.Contains(node.neighbours[i]) && graph != null)
                    {
                        int distanceToGoal =
                            (int)graph.GetNodeDistance(node.neighbours[i], goalNode);

                        node.neighbours[i].priority =
                            (int)node.neighbours[i].distanceTraveled + distanceToGoal;

                        frontierNodes.Enqueue(node.neighbours[i]);
                    }
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

    private void ShowDiagnostics(bool lerpColor = false, float lerpValue = 0.5f)
    {
        if (showColors)
        {
            ShowColors(lerpColor, lerpValue);
        }

        if (showArrows && graphView != null)
        {
            graphView.ShowNodeArrows(frontierNodes.ToList(), arrowColor);

            if (frontierNodes.Contains(goalNode))
            {
                graphView.ShowNodeArrows(pathNodes, highlightColor);
            }
        }
    }

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

        frontierNodes = new PriorityQueue<Node>();
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

        startNode.distanceTraveled = 0;
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        float timeStart = Time.time;

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

                if (mode == Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if (mode == Mode.GreedyBestFirst)
                {
                    ExpandFrontierGreedyBestFirst(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {
                    ExpandFrontierDijkstra(currentNode);
                }
                else if (mode == Mode.AStar)
                {
                    ExpandFrontierAStar(currentNode);
                }

                if (frontierNodes.Contains(goalNode))
                {
                    pathNodes = GetPathNodes(goalNode);

                    if (exitOnGoal)
                    {
                        isComplete = true;

                        Debug.Log("Pathfinder mode:" + mode.ToString() + ", path length = " +
                                  goalNode.distanceTraveled.ToString());
                    }
                }

                if (showIterations)
                {
                    ShowDiagnostics(true);

                    yield return new WaitForSeconds(timeStep);
                }
            }
            else
            {
                isComplete = true;
            }
        }

        ShowDiagnostics(true);

        Debug.Log("Pathfinder: SearchRoutine: elapse time = " + (Time.time - timeStart).ToString() +
                  " seconds");
    }
}
