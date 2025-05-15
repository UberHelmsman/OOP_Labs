using lab2;

// Загрузка шрифта (создайте файл font.txt в формате как в задании)
Printer.SetFont("font.txt");

// Статическое использование
Printer.Print("HELLO", Color.Red, (5, 5), '#');
Printer.Print("WORLD", Color.BrightGreen, (5, 10), '@');

// Использование с using
using (var printer = new Printer(Color.Cyan, (5, 15), '+'))
{
    printer.Print("CSharp");
    printer.Print("Rocks");
}

// После using цвета и позиция курсора восстановлены
Console.WriteLine("Back to normal console output.");