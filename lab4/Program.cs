using System;
using lab4;


var keyboard = new VirtualKeyboard();
    
    Console.WriteLine("Virtual Keyboard Demo");
    Console.WriteLine("====================");
    
    // Демонстрация печати символов
    Console.WriteLine("\n--- Typing characters ---");
    keyboard.ProcessKey("h");
    keyboard.ProcessKey("e");
    keyboard.ProcessKey("l");
    keyboard.ProcessKey("l");
    keyboard.ProcessKey("o");
    
    // Демонстрация undo/redo
    Console.WriteLine("\n--- Undo/Redo operations ---");
    keyboard.Undo(); // убираем 'o'
    keyboard.Undo(); // убираем 'l'
    keyboard.Redo(); // возвращаем 'l'
    
    // Демонстрация команд громкости
    Console.WriteLine("\n--- Volume control ---");
    keyboard.ProcessKey("ctrl++"); // увеличиваем громкость
    keyboard.ProcessKey("ctrl++"); // еще раз увеличиваем
    keyboard.ProcessKey("ctrl+-"); // уменьшаем громкость
    keyboard.Undo(); // отменяем уменьшение
    
    // Демонстрация медиа плеера
    Console.WriteLine("\n--- Media player control ---");
    keyboard.ProcessKey("ctrl+p"); // запускаем плеер
    keyboard.ProcessKey("ctrl+p"); // останавливаем плеер
    keyboard.Undo(); // отменяем остановку
    
    // Показываем текущее состояние
    keyboard.ShowStatus();
    
    // Интерактивный режим
    Console.WriteLine("\n--- Interactive mode ---");
    Console.WriteLine("Enter commands (type 'help' for available commands, 'exit' to quit):");
    
    string input;
    while ((input = Console.ReadLine()) != "exit")
    {
        switch (input?.ToLower())
        {
            case "help":
                ShowHelp();
                break;
            case "status":
                keyboard.ShowStatus();
                break;
            case "bindings":
                keyboard.ShowKeyBindings();
                break;
            case "undo":
                keyboard.Undo();
                break;
            case "redo":
                keyboard.Redo();
                break;
            case "save":
                keyboard.SaveState();
                break;
            case "load":
                keyboard.LoadState();
                break;
            case "clear":
                keyboard.ClearTextBuffer();
                break;
            case "":
            case null:
                continue;
            default:
                keyboard.ProcessKey(input);
                break;
        }
    }
    
    keyboard.Shutdown();
    Console.WriteLine("Program ended.");


static void ShowHelp()
{
    Console.WriteLine("\nAvailable commands:");
    Console.WriteLine("  a-z, 0-9, space - Type characters");
    Console.WriteLine("  ctrl++           - Increase volume");
    Console.WriteLine("  ctrl+-           - Decrease volume");
    Console.WriteLine("  ctrl+p           - Toggle media player");
    Console.WriteLine("  undo             - Undo last command");
    Console.WriteLine("  redo             - Redo last undone command");
    Console.WriteLine("  status           - Show current status");
    Console.WriteLine("  bindings         - Show key bindings");
    Console.WriteLine("  save             - Save current state");
    Console.WriteLine("  load             - Load saved state");
    Console.WriteLine("  clear            - Clear text buffer");
    Console.WriteLine("  help             - Show this help");
    Console.WriteLine("  exit             - Exit program");
    Console.WriteLine();
    }
