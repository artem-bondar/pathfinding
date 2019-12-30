using System;
using System.Collections.Generic;

using UnityEngine;

public class MapData : MonoBehaviour
{
    public int width = 0;
    public int height = 0;

    public TextAsset textAsset;

    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach (string line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();
        lines = GetTextFromFile();
        SetDimensions(lines);

        int[,] map = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
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
            string[] delimiters = { "\r\n", "\n" };

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
