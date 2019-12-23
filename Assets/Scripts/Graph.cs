using System.Collections.Generic;

using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;

    public List<Node> walls = new List<Node>();

    private int[,] mapData;

    private int width;
    private int height;

    public void Init(int[,] mapData)
    {
        this.mapData = mapData;

        width = mapData.GetLength(0);
        width = mapData.GetLength(1);

        nodes = new Node[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                NodeType type = (NodeType) mapData[x, y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x, 0, y);

                if (type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }
    }
}
