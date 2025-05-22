using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace lab2;
public static class FontLoader
{
    private static Dictionary<char, string[]>? _font;
    
    public static Dictionary<char, string[]> LoadFont(string fontPath = "font.json")
    {
        if (_font != null) return _font;
        
        try
        {
            if (!File.Exists(fontPath))
            {
                CreateDefaultFont(fontPath);
            }
            
            string json = File.ReadAllText(fontPath);
            var fontData = JsonSerializer.Deserialize<Dictionary<string, string[]>>(json);
            
            _font = new Dictionary<char, string[]>();
            foreach (var kvp in fontData)
            {
                if (kvp.Key.Length == 1)
                {
                    _font[kvp.Key[0]] = kvp.Value;
                }
            }
            
            return _font;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки шрифта: {ex.Message}");
            return CreateEmptyFont();
        }
    }
    
    private static void CreateDefaultFont(string fontPath)
    {
        var defaultFont = new Dictionary<string, string[]>
        {
            ["A"] = new[] { " ***  ", "*   * ", "***** ", "*   * ", "*   * " },
            ["B"] = new[] { "****  ", "*   * ", "****  ", "*   * ", "****  " },
            ["C"] = new[] { " **** ", "*     ", "*     ", "*     ", " **** " },
            ["D"] = new[] { "****  ", "*   * ", "*   * ", "*   * ", "****  " },
            ["E"] = new[] { "***** ", "*     ", "***   ", "*     ", "***** " },
            ["F"] = new[] { "***** ", "*     ", "***   ", "*     ", "*     " },
            ["G"] = new[] { " **** ", "*     ", "* *** ", "*   * ", " **** " },
            ["H"] = new[] { "*   * ", "*   * ", "***** ", "*   * ", "*   * " },
            ["I"] = new[] { "***** ", "  *   ", "  *   ", "  *   ", "***** " },
            ["J"] = new[] { "***** ", "    * ", "    * ", "*   * ", " ***  " },
            ["K"] = new[] { "*   * ", "*  *  ", "**    ", "*  *  ", "*   * " },
            ["L"] = new[] { "*     ", "*     ", "*     ", "*     ", "***** " },
            ["M"] = new[] { "*   * ", "** ** ", "* * * ", "*   * ", "*   * " },
            ["N"] = new[] { "*   * ", "**  * ", "* * * ", "*  ** ", "*   * " },
            ["O"] = new[] { " ***  ", "*   * ", "*   * ", "*   * ", " ***  " },
            ["P"] = new[] { "****  ", "*   * ", "****  ", "*     ", "*     " },
            ["Q"] = new[] { " ***  ", "*   * ", "* * * ", "*  ** ", " **** " },
            ["R"] = new[] { "****  ", "*   * ", "****  ", "*  *  ", "*   * " },
            ["S"] = new[] { " **** ", "*     ", " ***  ", "    * ", "****  " },
            ["T"] = new[] { "***** ", "  *   ", "  *   ", "  *   ", "  *   " },
            ["U"] = new[] { "*   * ", "*   * ", "*   * ", "*   * ", " ***  " },
            ["V"] = new[] { "*   * ", "*   * ", "*   * ", " * *  ", "  *   " },
            ["W"] = new[] { "*   * ", "*   * ", "* * * ", "** ** ", "*   * " },
            ["X"] = new[] { "*   * ", " * *  ", "  *   ", " * *  ", "*   * " },
            ["Y"] = new[] { "*   * ", " * *  ", "  *   ", "  *   ", "  *   " },
            ["Z"] = new[] { "***** ", "   *  ", "  *   ", " *    ", "***** " },
            [" "] = new[] { "      ", "      ", "      ", "      ", "      " }
        };
        
        string json = JsonSerializer.Serialize(defaultFont, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fontPath, json);
    }
    
    private static Dictionary<char, string[]> CreateEmptyFont()
    {
        return new Dictionary<char, string[]>
        {
            [' '] = new[] { "      ", "      ", "      ", "      ", "      " }
        };
    }
}
