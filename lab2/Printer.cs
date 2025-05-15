namespace lab2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



public class Printer : IDisposable
{
    private static Font _font;
    private readonly Color _color;
    private readonly (int left, int top) _position;
    private readonly char _symbol;
    private readonly ConsoleColor _originalForegroundColor;
    private readonly ConsoleColor _originalBackgroundColor;

    public Printer(Color color, (int left, int top) position, char symbol = '*')
    {
        _color = color;
        _position = position;
        _symbol = symbol;
        _originalForegroundColor = Console.ForegroundColor;
        _originalBackgroundColor = Console.BackgroundColor;
    }

    public static void SetFont(string fontFilePath)
    {
        _font = new Font(fontFilePath);
    }

    public static void Print(string text, Color color, (int left, int top) position, char symbol = '*')
    {
        if (_font == null)
        {
            throw new InvalidOperationException("Font is not loaded. Call SetFont first.");
        }

        int currentTop = position.top;

        // Для каждого символа в тексте
        foreach (char c in text)
            {
                var charLines = _font.GetCharacter(c, _symbol);
                int charWidth = 0;

                foreach (var line in charLines)
                {
                    Console.SetCursorPosition(currentLeft, currentTop);
                    SetColor(_color);
                    Console.Write(line);
                    currentTop++;
                    charWidth = Math.Max(charWidth, line.Length);
                }

                currentTop -= charLines.Length;
                currentLeft += charWidth + 1;
            }

        ResetColor();
    }

    public void Print(string text)
    {
        Print(text, _color, _position, _symbol);
    }

    private static void SetColor(Color color)
    {
        Console.Write($"\x1b[{(int)color}m");
    }

    private static void ResetColor()
    {
        Console.Write("\x1b[0m");
    }

    public void Dispose()
    {
        Console.ForegroundColor = _originalForegroundColor;
        Console.BackgroundColor = _originalBackgroundColor;
        Console.SetCursorPosition(0, _position.top + _font.Height + 1);
    }
}