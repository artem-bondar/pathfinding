using System.Collections.Generic;

using UnityEngine;

public class MapData : MonoBehaviour
{
    public int width = 10;
    public int height = 5;

    public TextAsset textAsset;

    public int[,] MakeMap()
    {
        int[,] map = new int [width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = 0;
            }
        }
    
        return map;
    }

    public List<string> GetTextFromFile(TextAsset textAsset)
    {
        List<string> lines = new List<string>();

        if (textAsset != null)
        {
            string textData = textAsset.text;
            string[] delimiters = { "\r\n", "\n"};

            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MapData: GetTextFromFile Error: invalid TextAsset!");
        }

        return lines;
    }

    public List<string> GetTextFromFile() => GetTextFromFile(textAsset);
}
