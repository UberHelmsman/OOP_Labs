using System;

namespace lab6;


/// <summary>
/// Команда увеличения громкости звука.
/// Реализует паттерн Command для управления громкостью.
/// </summary>
public class VolumeUpCommand : ICommand
{
    /// <summary>
    /// Функция получения текущей громкости
    /// </summary>
    private readonly Func<int> _getCurrentVolume;

    /// <summary>
    /// Функция установки громкости
    /// </summary>
    private readonly Action<int> _setVolume;

    /// <summary>
    /// Предыдущий уровень громкости для операции отмены
    /// </summary>
    private int _previousVolume;

    /// <summary>
    /// Новый уровень громкости после выполнения команды
    /// </summary>
    private int _newVolume;

    /// <summary>
    /// Шаг изменения громкости (в процентах)
    /// </summary>
    private const int VOLUME_STEP = 10;

    /// <summary>
    /// Инициализирует новый экземпляр команды увеличения громкости.
    /// </summary>
    /// <param name="getCurrentVolume">Функция получения текущей громкости</param>
    /// <param name="setVolume">Функция установки громкости</param>
    public VolumeUpCommand(Func<int> getCurrentVolume, Action<int> setVolume)
    {
        _getCurrentVolume = getCurrentVolume;
        _setVolume = setVolume;
    }

    /// <summary>
    /// Выполняет команду увеличения громкости.
    /// Увеличивает громкость на заданный шаг, но не более 100%.
    /// </summary>
    public void Execute()
    {
        _previousVolume = _getCurrentVolume();
        _newVolume = Math.Min(100, _previousVolume + VOLUME_STEP);
        _setVolume(_newVolume);
        Console.WriteLine($"Volume increased to {_newVolume}%");
    }

    /// <summary>
    /// Отменяет команду увеличения громкости.
    /// Восстанавливает предыдущий уровень громкости.
    /// </summary>
    public void Undo()
    {
        _setVolume(_previousVolume);
        Console.WriteLine($"Volume restored to {_previousVolume}%");
    }

    /// <summary>
    /// Получает описание команды для логирования.
    /// </summary>
    /// <returns>Строка с информацией о новом уровне громкости</returns>
    public string GetDescription()
    {
        return $"volume increased to {_newVolume}%";
    }
}

/// <summary>
/// Команда уменьшения громкости звука.
/// Реализует паттерн Command для управления громкостью.
/// </summary>
public class VolumeDownCommand : ICommand
{
    /// <summary>
    /// Функция получения текущей громкости
    /// </summary>
    private readonly Func<int> _getCurrentVolume;
    
    /// <summary>
    /// Функция установки громкости
    /// </summary>
    private readonly Action<int> _setVolume;
    
    /// <summary>
    /// Предыдущий уровень громкости для операции отмены
    /// </summary>
    private int _previousVolume;
    
    /// <summary>
    /// Новый уровень громкости после выполнения команды
    /// </summary>
    private int _newVolume;
    
    /// <summary>
    /// Шаг изменения громкости (в процентах)
    /// </summary>
    private const int VOLUME_STEP = 10;
    
    /// <summary>
    /// Инициализирует новый экземпляр команды уменьшения громкости.
    /// </summary>
    /// <param name="getCurrentVolume">Функция получения текущей громкости</param>
    /// <param name="setVolume">Функция установки громкости</param>
    public VolumeDownCommand(Func<int> getCurrentVolume, Action<int> setVolume)
    {
        _getCurrentVolume = getCurrentVolume;
        _setVolume = setVolume;
    }
    
    /// <summary>
    /// Выполняет команду уменьшения громкости.
    /// Уменьшает громкость на заданный шаг, но не менее 0%.
    /// </summary>
    public void Execute()
    {
        _previousVolume = _getCurrentVolume();
        _newVolume = Math.Max(0, _previousVolume - VOLUME_STEP);
        _setVolume(_newVolume);
        Console.WriteLine($"Volume decreased to {_newVolume}%");
    }
    
    /// <summary>
    /// Отменяет команду уменьшения громкости.
    /// Восстанавливает предыдущий уровень громкости.
    /// </summary>
    public void Undo()
    {
        _setVolume(_previousVolume);
        Console.WriteLine($"Volume restored to {_previousVolume}%");
    }
    
    /// <summary>
    /// Получает описание команды для логирования.
    /// </summary>
    /// <returns>Строка с информацией о новом уровне громкости</returns>
    public string GetDescription()
    {
        return $"volume decreased to {_newVolume}%";
    }
}