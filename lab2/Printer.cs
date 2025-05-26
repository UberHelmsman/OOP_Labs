using System;
using System.Collections.Generic;

namespace lab2;


public class Printer : IDisposable
{
    private static string prefix = "\u001b[";
    private readonly Color _fixedColor;
    private readonly (int x, int y) _fixedPosition;
    private readonly char _fixedSymbol;
    private readonly bool _isInstance;
    private readonly string _originalCursorState;
    private static Dictionary<char, string[]> Font = FontLoader.LoadFont();


    public Printer(Color color, (int x, int y) position, char symbol = '*', string FontPath = "font.json")
    {
        _fixedColor = color;
        _fixedPosition = position;
        _fixedSymbol = symbol;
        _isInstance = true;
        Font = FontLoader.LoadFont(FontPath);
        

        _originalCursorState = $"{prefix}{Console.CursorTop};{Console.CursorLeft}H"; // место палки в терминале
        

        Console.Write($"{prefix}{(int)_fixedColor}m");//цвет
    }

    public static void Print(string text, Color color, (int x, int y) position, char symbol = '*')
    {

        var originalPosition = (Console.CursorLeft, Console.CursorTop);//палка в терминале
        

        Console.Write($"{prefix}{(int)color}m");
        

        PrintBigText(text, position, symbol);
        
        Console.Write("\u001b[0m"); // сброс цвета
        //Console.Write($"{prefix}"+"0m");
        
        Console.SetCursorPosition(originalPosition.CursorLeft, originalPosition.CursorTop);//обратно палку возвращаем
    }

    // метод для экземпляра
    public void Print(string text)
    {
        if (!_isInstance)
            throw new InvalidOperationException("Метод может быть вызван только на экземпляре класса");
        
        PrintBigText(text, _fixedPosition, _fixedSymbol);
    }

     // основной метод принтинга
    private static void PrintBigText(string text, (int x, int y) position, char symbol)
    {
        text = text.ToUpper();
        char c1 = (char)text[0];
        int fontHeight = Font[c1].Length;

        for (int row = 0; row < fontHeight; row++)
        {
            Console.SetCursorPosition(position.x, position.y + row);
            
            foreach (char c in text)
            {
                if (Font.ContainsKey(c))
                {
                    string pattern = Font[c][row];
                    pattern = pattern.Replace('*', symbol);
                    Console.Write(pattern);
                }
                else
                {
                    Console.Write("     ");
                }
            }
        }
    }

    
    // это нейросетка подсказала так сделать
    public void Dispose()
    {
        if (_isInstance)
        {
            Console.Write("\u001b[0m"); // сброс цвета
            //Console.Write($"{prefix}"+"0m"); // сброс цвета
            
            Console.Write(_originalCursorState); //восстановление положения курсора
        }
    }
}
