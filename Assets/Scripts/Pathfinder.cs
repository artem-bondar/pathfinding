﻿using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Node startNode;
    private Node goalNode;

    private Graph graph;
    private GraphView graphView;

    private Queue<Node> frontierNodes;

    private List<Node> exploreNodes;
    private List<Node> pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

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

        exploreNodes = new List<Node>();
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

        if (exploreNodes != null)
        {
            graphView.ColorNodes(exploreNodes, exploredColor);
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
}
