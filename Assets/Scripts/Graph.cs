using System.Collections.Generic;

using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;

    public List<Node> walls = new List<Node>();

    private int[,] mapData;

    private int width;
    private int height;

    public static readonly Vector2[] allDirections =
    {
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(1f, -1f),
        new Vector2(0f, -1f),
        new Vector2(-1f, -1f),
        new Vector2(-1f, 0f),
        new Vector2(-1f, 1f)
    };

    private List<Node> GetNeighbours(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighbourNodes = new List<Node>();

        foreach (Vector2 direction in directions)
        {
            int newX = x + (int)direction.x;
            int newY = y + (int)direction.y;

            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null &&
                nodeArray[newX, newY].nodeType != NodeType.Blocked)
            {
                neighbourNodes.Add(nodeArray[newX, newY]);
            }
        }

        return neighbourNodes;
    }

    private List<Node> GetNeighbours(int x, int y) => GetNeighbours(x, y, nodes, allDirections);

    public void Init(int[,] mapData)
    {
        this.mapData = mapData;

        width = mapData.GetLength(0);
        height = mapData.GetLength(1);

        nodes = new Node[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                NodeType type = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x, 0, y);

                if (type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (nodes[x, y].nodeType != NodeType.Blocked)
                {
                    nodes[x, y].neighbours = GetNeighbours(x, y);
                }
            }
        }
    }

    public bool IsWithinBounds(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;
}
