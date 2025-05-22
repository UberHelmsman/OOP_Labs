using System;
using System.Threading;
using lab2;


Console.Clear();



Printer.Print("oop is nice and csharp is nice", Color.BrightBlack,(0,1));

/*
//статический метод
Console.WriteLine("Статический вывод:");
Printer.Print("HELLO", Color.BrightRed, (5, 3), '#');

// перемещение палки
Console.SetCursorPosition(0, 10);
Console.WriteLine("Статический вывод завершен. Нажмите любую клавишу...");
Console.ReadKey();

Console.Clear();
Console.WriteLine("Вывод с использованием экземпляра класса (using):");

// с юзингом (это для чего то с мемори манагемент)
using (var printer = new Printer(Color.BrightGreen, (10, 3), '@'))
{
    printer.Print("WORLD");

    Console.SetCursorPosition(0, 10);
    Console.WriteLine("Первое слово выведено");
    Thread.Sleep(1000);

    // позицию меняем
    Console.SetCursorPosition(0, 12);
    Console.WriteLine("Выводим второе слово");
    Thread.Sleep(1000);
}


Console.SetCursorPosition(0, 15);
Console.WriteLine("Цвет автоматически сброшен после using.");
Console.ReadKey();

// разные цвета
Console.Clear();
Console.WriteLine("Демонстрация разных цветов и символов:");

Printer.Print("RED", Color.BrightRed, (0, 2), '*');
Printer.Print("BLUE", Color.BrightBlue, (0, 8), '+');
Printer.Print("GREEN", Color.BrightGreen, (0, 14), '#');

Console.SetCursorPosition(0, 20);
*/