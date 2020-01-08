using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    public int width = 0;
    public int height = 0;

    public TextAsset textAsset;
    public Texture2D textureMap;

    public string resourcePath = "Mapdata";

    private void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (textureMap == null)
        {
            textureMap = Resources.Load<Texture2D>(resourcePath + "/" + levelName);
        }

        if (textAsset == null)
        {
            textAsset = Resources.Load<TextAsset>(resourcePath + "/" + levelName);
        }
    }

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

        if (textureMap != null)
        {
            lines = GetMapFromTexture(textureMap);
        }
        else
        {
            lines = GetMapFromTextFile(textAsset);
        }

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

    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string newLine = "";

                for (int x = 0; x < texture.width; x++)
                {
                    if (texture.GetPixel(x, y) == Color.black)
                    {
                        newLine += "1";
                    }
                    else if (texture.GetPixel(x, y) == Color.white)
                    {
                        newLine += "0";
                    }
                    else
                    {
                        newLine += " ";
                    }
                }

                lines.Add(newLine);
            }
        }

        return lines;
    }

    public List<string> GetMapFromTextFile(TextAsset textAsset)
    {
        List<string> lines = new List<string>();

        if (textAsset != null)
        {
            string textData = textAsset.text;
            string[] delimiters = { "\r\n", "\n" };

            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }

        return lines;
    }

    public List<string> GetTextFromFile() => GetMapFromTextFile(textAsset);
}
