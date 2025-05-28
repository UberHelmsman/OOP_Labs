using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace lab6;

/// <summary>
/// Класс виртуальной клавиатуры, реализующий паттерн Command.
/// Поддерживает назначение команд на клавиши, операции undo/redo и сохранение состояния.
/// </summary>
public class VirtualKeyboard
{
    /// <summary>
    /// Словарь ассоциаций клавиш с командами
    /// </summary>
    private Dictionary<string, Func<ICommand>> _keyBindings;

    /// <summary>
    /// Стек выполненных команд для операции undo
    /// </summary>
    private Stack<ICommand> _undoStack;

    /// <summary>
    /// Стек отмененных команд для операции redo
    /// </summary>
    private Stack<ICommand> _redoStack;

    /// <summary>
    /// Текстовый буфер для команд печати символов
    /// </summary>
    private StringBuilder _textBuffer;

    /// <summary>
    /// Текущая громкость звука
    /// </summary>
    private int _currentVolume;

    /// <summary>
    /// Состояние медиа плеера (запущен/остановлен)
    /// </summary>
    private bool _isMediaPlayerRunning;

    /// <summary>
    /// Класс для сохранения и восстановления состояния
    /// </summary>
    private KeyboardStateSaver _stateSaver;

    /// <summary>
    /// Путь к файлу для логирования операций
    /// </summary>
    private const string LOG_FILE = "keyboard_log.txt";

    /// <summary>
    /// Путь к файлу с биндингами клавиш
    /// </summary>
    private const string BINDINGS_FILE = "key_bindings.json";

    /// <summary>
    /// Инициализирует новый экземпляр виртуальной клавиатуры.
    /// </summary>
    public VirtualKeyboard()
    {
        _keyBindings = new Dictionary<string, Func<ICommand>>();
        _undoStack = new Stack<ICommand>();
        _redoStack = new Stack<ICommand>();
        _textBuffer = new StringBuilder();
        _currentVolume = 50;
        _isMediaPlayerRunning = false;
        _stateSaver = new KeyboardStateSaver();

        // Очищаем лог файл при запуске
        ClearLogFile();

        LoadKeyBindings();
        LoadState();
    }

    /// <summary>
    /// Очищает файл логирования при запуске программы.
    /// </summary>
    private void ClearLogFile()
    {
        try
        {
            File.WriteAllText(LOG_FILE, "");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing log file: {ex.Message}");
        }
    }

    /// <summary>
    /// Загружает биндинги клавиш из JSON файла.
    /// </summary>
    private void LoadKeyBindings()
    {
        try
        {
            if (!File.Exists(BINDINGS_FILE))
            {
                CreateDefaultBindingsFile();
            }

            string json = File.ReadAllText(BINDINGS_FILE);
            var bindingsConfig = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (bindingsConfig != null)
            {
                // Очищаем текущие биндинги
                _keyBindings.Clear();
                
                foreach (var binding in bindingsConfig)
                {
                    string key = binding.Key;
                    string commandType = binding.Value;

                    CreateCommandBinding(key, commandType);
                }

                Console.WriteLine("Key bindings loaded successfully");
                LogOperation("Key bindings loaded from file");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading key bindings: {ex.Message}");
            CreateDefaultBindingsInMemory();
        }
    }

    /// <summary>
    /// Создает биндинг команды на основе ключа и типа команды.
    /// </summary>
    /// <param name="key">Клавиша или диапазон</param>
    /// <param name="commandType">Тип команды</param>
    private void CreateCommandBinding(string key, string commandType)
    {
        switch (commandType)
        {
            case "volume_up":
                _keyBindings[key] = () => new VolumeUpCommand(
                    () => _currentVolume,
                    vol => _currentVolume = vol
                );
                break;
                
            case "volume_down":
                _keyBindings[key] = () => new VolumeDownCommand(
                    () => _currentVolume,
                    vol => _currentVolume = vol
                );
                break;
                
            case "media_player":
                _keyBindings[key] = () => new MediaPlayerCommand(
                    () => _isMediaPlayerRunning,
                    state => _isMediaPlayerRunning = state
                );
                break;
                
            case "print_char":
                HandlePrintCharBinding(key);
                break;
                
            default:
                Console.WriteLine($"Unknown command type: {commandType} for key: {key}");
                break;
        }
    }

    /// <summary>
    /// Обрабатывает биндинги для печати символов, включая диапазоны.
    /// </summary>
    /// <param name="key">Ключ биндинга</param>
    private void HandlePrintCharBinding(string key)
    {
        if (key == "letters:a-z")
        {
            // Создаем биндинги для всех букв
            for (char c = key[key.Length-2]; c <= key[key.Length]; c++)
            {
                char ch = c;
                _keyBindings[ch.ToString()] = () => new PrintCharacterCommand(ch, _textBuffer);
            }
        }
        else if (key == "digits:0-9")
        {
            // Создаем биндинги для всех цифр
            for (char c = key[key.Length-2]; c <= key[key.Length]; c++)
            {
                char ch = c;
                _keyBindings[ch.ToString()] = () => new PrintCharacterCommand(ch, _textBuffer);
            }
        }
        else if (key == " " || key == "space")
        {
            _keyBindings[" "] = () => new PrintCharacterCommand(' ', _textBuffer);
            _keyBindings["space"] = () => new PrintCharacterCommand(' ', _textBuffer);
        }
        else if (key.Length == 1)
        {
            char ch = key[0];
            _keyBindings[key] = () => new PrintCharacterCommand(ch, _textBuffer);
        }
        else
        {
            Console.WriteLine($"Cannot create print char binding for key: {key}");
        }
    }

    /// <summary>
    /// Создает файл с биндингами по умолчанию.
    /// </summary>
    private void CreateDefaultBindingsFile()
    {
        var defaultBindings = new Dictionary<string, string>
        {
            // Системные команды
            ["ctrl++"] = "volume_up",
            ["ctrl+-"] = "volume_down",
            ["ctrl+p"] = "media_player",
            
            // Символы для печати - используем диапазоны для компактности
            ["letters:a-z"] = "print_char",
            ["digits:0-9"] = "print_char",
            ["space"] = "print_char"
        };

        SaveKeyBindingsToFile(defaultBindings);
        Console.WriteLine("Default key bindings file created");
    }

    /// <summary>
    /// Создает биндинги по умолчанию в памяти (на случай ошибки чтения файла).
    /// </summary>
    private void CreateDefaultBindingsInMemory()
    {
        Console.WriteLine("Creating default bindings in memory");
        
        // Системные команды
        _keyBindings["ctrl++"] = () => new VolumeUpCommand(
            () => _currentVolume,
            vol => _currentVolume = vol
        );

        _keyBindings["ctrl+-"] = () => new VolumeDownCommand(
            () => _currentVolume,
            vol => _currentVolume = vol
        );

        _keyBindings["ctrl+p"] = () => new MediaPlayerCommand(
            () => _isMediaPlayerRunning,
            state => _isMediaPlayerRunning = state
        );

        // Символы алфавита
        for (char c = 'a'; c <= 'z'; c++)
        {
            char ch = c;
            _keyBindings[ch.ToString()] = () => new PrintCharacterCommand(ch, _textBuffer);
        }

        // Цифры
        for (char c = '0'; c <= '9'; c++)
        {
            char ch = c;
            _keyBindings[ch.ToString()] = () => new PrintCharacterCommand(ch, _textBuffer);
        }

        // Пробел
        _keyBindings[" "] = () => new PrintCharacterCommand(' ', _textBuffer);
        _keyBindings["space"] = () => new PrintCharacterCommand(' ', _textBuffer);
    }

    /// <summary>
    /// Сохраняет текущие биндинги клавиш в JSON файл.
    /// </summary>
    public void SaveKeyBindings()
    {
        try
        {
            var bindingsConfig = new Dictionary<string, string>();

            // Добавляем системные команды
            foreach (var binding in _keyBindings)
            {
                var command = binding.Value();
                string commandType = command switch
                {
                    VolumeUpCommand => "volume_up",
                    VolumeDownCommand => "volume_down", 
                    MediaPlayerCommand => "media_player",
                    PrintCharacterCommand => "print_char",
                    _ => "unknown"
                };

                // Для символов группируем в диапазоны
                if (commandType == "print_char")
                {
                    if (binding.Key.Length == 1 && char.IsLetter(binding.Key[0]))
                    {
                        // Не добавляем отдельные буквы, они будут в диапазоне
                        continue;
                    }
                    else if (binding.Key.Length == 1 && char.IsDigit(binding.Key[0]))
                    {
                        // Не добавляем отдельные цифры, они будут в диапазоне
                        continue;
                    }
                }

                bindingsConfig[binding.Key] = commandType;
            }

            // Добавляем диапазоны для компактности
            bool hasLetters = _keyBindings.ContainsKey("a");
            bool hasDigits = _keyBindings.ContainsKey("0");
            
            if (hasLetters)
                bindingsConfig["letters:a-z"] = "print_char";
            if (hasDigits)
                bindingsConfig["digits:0-9"] = "print_char";

            SaveKeyBindingsToFile(bindingsConfig);
            Console.WriteLine("Key bindings saved successfully");
            LogOperation("Key bindings saved to file");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving key bindings: {ex.Message}");
        }
    }

    /// <summary>
    /// Сохраняет словарь биндингов в JSON файл.
    /// </summary>
    /// <param name="bindings">Словарь биндингов для сохранения</param>
    private void SaveKeyBindingsToFile(Dictionary<string, string> bindings)
    {
        string json = JsonSerializer.Serialize(bindings, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(BINDINGS_FILE, json);
    }

    /// <summary>
    /// Добавляет или изменяет привязку клавиши к команде.
    /// </summary>
    /// <param name="key">Клавиша или комбинация клавиш</param>
    /// <param name="commandFactory">Фабрика для создания команды</param>
    public void AddKeyBinding(string key, Func<ICommand> commandFactory)
    {
        _keyBindings[key] = commandFactory;
        LogOperation($"Key binding added: {key}");
    }

    /// <summary>
    /// Удаляет привязку клавиши.
    /// </summary>
    /// <param name="key">Клавиша для удаления привязки</param>
    public void RemoveKeyBinding(string key)
    {
        if (_keyBindings.ContainsKey(key))
        {
            _keyBindings.Remove(key);
            LogOperation($"Key binding removed: {key}");
        }
    }

    /// <summary>
    /// Обрабатывает нажатие клавиши и выполняет соответствующую команду.
    /// </summary>
    /// <param name="key">Нажатая клавиша или комбинация</param>
    public void ProcessKey(string key)
    {
        if (_keyBindings.ContainsKey(key))
        {
            var command = _keyBindings[key]();
            ExecuteCommand(command);
        }
        else
        {
            Console.WriteLine($"No command bound to key: {key}");
            LogOperation($"No command bound to key: {key}");
        }
    }

    /// <summary>
    /// Выполняет команду и добавляет ее в стек для возможности отмены.
    /// </summary>
    /// <param name="command">Команда для выполнения</param>
    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear(); // Очищаем redo стек при выполнении новой команды

        // Логируем операцию
        string description = command.GetDescription();
        LogOperation(description);

        // Если это команда печати, выводим текущий буфер
        if (command is PrintCharacterCommand)
        {
            Console.WriteLine($"Text buffer: {_textBuffer}");
            LogOperation($"Text buffer: {_textBuffer}");
        }
    }

    /// <summary>
    /// Отменяет последнюю выполненную команду (undo).
    /// </summary>
    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);

            Console.WriteLine("undo");
            LogOperation("undo");

            // Если это команда печати, выводим текущий буфер
            if (command is PrintCharacterCommand)
            {
                Console.WriteLine($"Text buffer: {_textBuffer}");
                LogOperation($"Text buffer: {_textBuffer}");
            }
        }
        else
        {
            Console.WriteLine("Nothing to undo");
            LogOperation("Nothing to undo");
        }
    }

    /// <summary>
    /// Повторяет последнюю отмененную команду (redo).
    /// </summary>
    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);

            Console.WriteLine("redo");
            LogOperation("redo");

            // Если это команда печати, выводим текущий буфер
            if (command is PrintCharacterCommand)
            {
                Console.WriteLine($"Text buffer: {_textBuffer}");
                LogOperation($"Text buffer: {_textBuffer}");
            }
        }
        else
        {
            Console.WriteLine("Nothing to redo");
            LogOperation("Nothing to redo");
        }
    }

    /// <summary>
    /// Сохраняет текущее состояние клавиатуры.
    /// </summary>
    public void SaveState()
    {
        var memento = new KeyboardMemento
        {
            Volume = _currentVolume,
            IsMediaPlayerRunning = _isMediaPlayerRunning,
            TextBuffer = _textBuffer.ToString()
        };

        _stateSaver.SaveState(memento);
        LogOperation("State saved");
    }

    /// <summary>
    /// Загружает сохраненное состояние клавиатуры.
    /// </summary>
    public void LoadState()
    {
        var memento = _stateSaver.LoadState();

        _currentVolume = memento.Volume;
        _isMediaPlayerRunning = memento.IsMediaPlayerRunning;
        _textBuffer.Clear();
        _textBuffer.Append(memento.TextBuffer);

        Console.WriteLine($"State loaded: Volume={_currentVolume}%, MediaPlayer={_isMediaPlayerRunning}, Text='{_textBuffer}'");
        LogOperation($"State loaded: Volume={_currentVolume}%, MediaPlayer={_isMediaPlayerRunning}, Text='{_textBuffer}'");
    }

    /// <summary>
    /// Логирует операцию в файл.
    /// </summary>
    /// <param name="operation">Описание операции</param>
    private void LogOperation(string operation)
    {
        try
        {
            using (var writer = new StreamWriter(LOG_FILE, true))
            {
                writer.WriteLine(operation);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    /// <summary>
    /// Получает текущий текстовый буфер.
    /// </summary>
    /// <returns>Содержимое текстового буфера</returns>
    public string GetTextBuffer()
    {
        return _textBuffer.ToString();
    }

    /// <summary>
    /// Получает текущую громкость.
    /// </summary>
    /// <returns>Уровень громкости в процентах</returns>
    public int GetVolume()
    {
        return _currentVolume;
    }

    /// <summary>
    /// Получает состояние медиа плеера.
    /// </summary>
    /// <returns>True, если плеер запущен, иначе false</returns>
    public bool IsMediaPlayerRunning()
    {
        return _isMediaPlayerRunning;
    }

    /// <summary>
    /// Очищает текстовый буфер.
    /// </summary>
    public void ClearTextBuffer()
    {
        _textBuffer.Clear();
        Console.WriteLine("Text buffer cleared");
        LogOperation("Text buffer cleared");
    }

    /// <summary>
    /// Отображает все доступные привязки клавиш.
    /// </summary>
    public void ShowKeyBindings()
    {
        Console.WriteLine("\nAvailable key bindings:");
        foreach (var binding in _keyBindings)
        {
            var command = binding.Value();
            Console.WriteLine($"  {binding.Key} -> {command.GetType().Name}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Отображает текущее состояние клавиатуры.
    /// </summary>
    public void ShowStatus()
    {
        Console.WriteLine($"\nCurrent status:");
        Console.WriteLine($"  Volume: {_currentVolume}%");
        Console.WriteLine($"  Media Player: {(_isMediaPlayerRunning ? "Running" : "Stopped")}");
        Console.WriteLine($"  Text Buffer: '{_textBuffer}'");
        Console.WriteLine($"  Undo Stack: {_undoStack.Count} commands");
        Console.WriteLine($"  Redo Stack: {_redoStack.Count} commands");
        Console.WriteLine();
    }

    /// <summary>
    /// Освобождает ресурсы и сохраняет состояние при завершении работы.
    /// </summary>
    public void Shutdown()
    {
        SaveState();
        Console.WriteLine("Keyboard shutdown complete");
        LogOperation("Keyboard shutdown complete");
    }
}