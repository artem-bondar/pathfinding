using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;
    public GameObject arrow;

    private Node node;

    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    private void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer goRenderer = go.GetComponent<Renderer>();

            if (goRenderer != null)
            {
                goRenderer.material.color = color;
            }
        }
    }

    private void EnableObject(GameObject gameObject, bool state)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(state);
        }
    }

    public void ShowArrow(Color color)
    {
        if (node != null && arrow != null && node.previous != null)
        {
            EnableObject(arrow, true);

            Vector3 directionToPrevious = (node.previous.position - node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(directionToPrevious);

            Renderer arrowRenderer = arrow.GetComponent<Renderer>();

            if (arrowRenderer != null)
            {
                arrowRenderer.material.color = color;
            }
        }
    }

    public void Init(Node node)
    {
        if (tile != null)
        {
            gameObject.name = $"Node ({node.xIndex}, {node.yIndex})";
            gameObject.transform.position = node.position;

            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);

            this.node = node;
            
            EnableObject(arrow, false);
        }
    }

    public void ColorNode(Color color) => ColorNode(color, tile);
}
