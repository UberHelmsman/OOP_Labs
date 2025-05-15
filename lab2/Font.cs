namespace lab2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class Font
{
    private readonly Dictionary<char, string[]> _characters;

    public Font(string fontFilePath)
    {
        _characters = new Dictionary<char, string[]>();
        LoadFont(fontFilePath);
    }

    private void LoadFont(string fontFilePath)
    {
        var lines = File.ReadAllLines(fontFilePath);
        char currentChar = '\0';
        var currentCharLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.Length == 1)
            {
                if (currentChar != '\0')
                {
                    _characters[currentChar] = currentCharLines.ToArray();
                    currentCharLines.Clear();
                }
                currentChar = line[0];
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                currentCharLines.Add(line);
            }
        }

        if (currentChar != '\0' && currentCharLines.Count > 0)
        {
            _characters[currentChar] = currentCharLines.ToArray();
        }
    }

    public string[] GetCharacter(char c, char symbol)
    {
        if (_characters.TryGetValue(char.ToUpper(c), out var charLines))
        {
            return charLines.Select(line => line.Replace('*', symbol)).ToArray();
        }
        return new[] { symbol.ToString() };
    }

    public int Height => _characters.Count > 0 ? _characters.First().Value.Length : 1;
}