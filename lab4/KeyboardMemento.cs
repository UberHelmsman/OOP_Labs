using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace lab4;
/// <summary>
/// Класс для хранения снимка состояния клавиатуры (паттерн Memento).
/// Содержит все данные, необходимые для восстановления состояния виртуальной клавиатуры.
/// </summary>
public class KeyboardMemento
{
    /// <summary>
    /// Словарь привязок клавиш к командам.
    /// Ключ - комбинация клавиш (например, "ctrl++"), значение - тип команды (например, "volume_up").
    /// </summary>
    public Dictionary<string, string> KeyBindings { get; set; } = new Dictionary<string, string>();
    
    /// <summary>
    /// Текущий уровень громкости (от 0 до 100).
    /// </summary>
    public int Volume { get; set; } = 50;
    
    /// <summary>
    /// Флаг состояния медиа плеера (запущен/остановлен).
    /// </summary>
    public bool IsMediaPlayerRunning { get; set; } = false;
    
    /// <summary>
    /// Содержимое текстового буфера.
    /// </summary>
    public string TextBuffer { get; set; } = "";
}

/// <summary>
/// Класс для сохранения и восстановления состояния виртуальной клавиатуры (паттерн Memento).
/// Отвечает за операции сериализации/десериализации состояния в JSON файл.
/// </summary>
public class KeyboardStateSaver
{
    /// <summary>
    /// Имя файла для сохранения состояния клавиатуры
    /// </summary>
    private const string SAVE_FILE = "keyboard_state.json";
    
    /// <summary>
    /// Сохраняет состояние клавиатуры в JSON файл.
    /// </summary>
    /// <param name="memento">Снимок состояния клавиатуры для сохранения</param>
    public void SaveState(KeyboardMemento memento)
    {
        try
        {
            string json = JsonSerializer.Serialize(memento, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            File.WriteAllText(SAVE_FILE, json);
            Console.WriteLine("Keyboard state saved successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving state: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Загружает состояние клавиатуры из JSON файла.
    /// Если файл не существует или произошла ошибка, возвращает состояние по умолчанию.
    /// </summary>
    /// <returns>Снимок состояния клавиатуры</returns>
    public KeyboardMemento LoadState()
    {
        try
        {
            if (!File.Exists(SAVE_FILE))
            {
                Console.WriteLine("No saved state found, using defaults");
                return new KeyboardMemento();
            }
            
            string json = File.ReadAllText(SAVE_FILE);
            var memento = JsonSerializer.Deserialize<KeyboardMemento>(json);
            Console.WriteLine("Keyboard state loaded successfully");
            return memento ?? new KeyboardMemento();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading state: {ex.Message}");
            return new KeyboardMemento();
        }
    }
}