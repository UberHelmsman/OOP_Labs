using System;

namespace lab6;


/// <summary>
/// Команда управления медиа плеером (запуск/остановка).
/// Реализует паттерн Command для управления медиа плеером.
/// </summary>
public class MediaPlayerCommand : ICommand
{
    /// <summary>
    /// Функция получения текущего состояния медиа плеера
    /// </summary>
    private readonly Func<bool> _getPlayerState;

    /// <summary>
    /// Функция установки состояния медиа плеера
    /// </summary>
    private readonly Action<bool> _setPlayerState;

    /// <summary>
    /// Предыдущее состояние медиа плеера для операции отмены
    /// </summary>
    private bool _previousState;

    /// <summary>
    /// Инициализирует новый экземпляр команды управления медиа плеером.
    /// </summary>
    /// <param name="getPlayerState">Функция получения состояния плеера</param>
    /// <param name="setPlayerState">Функция установки состояния плеера</param>
    public MediaPlayerCommand(Func<bool> getPlayerState, Action<bool> setPlayerState)
    {
        _getPlayerState = getPlayerState;
        _setPlayerState = setPlayerState;
    }

    /// <summary>
    /// Выполняет команду переключения состояния медиа плеера.
    /// Если плеер был остановлен - запускает, если был запущен - останавливает.
    /// </summary>
    public void Execute()
    {
        _previousState = _getPlayerState();
        bool newState = !_previousState;
        _setPlayerState(newState);

        if (newState)
        {
            Console.WriteLine("Media player launched");
        }
        else
        {
            Console.WriteLine("Media player stopped");
        }
    }

    /// <summary>
    /// Отменяет команду управления медиа плеером.
    /// Восстанавливает предыдущее состояние плеера.
    /// </summary>
    public void Undo()
    {
        _setPlayerState(_previousState);

        if (_previousState)
        {
            Console.WriteLine("Media player restored to running state");
        }
        else
        {
            Console.WriteLine("Media player restored to stopped state");
        }
    }

    /// <summary>
    /// Получает описание команды для логирования.
    /// </summary>
    /// <returns>Строка с информацией о текущем состоянии медиа плеера</returns>
    public string GetDescription()
    {
        return _getPlayerState() ? "media player launched" : "media player stopped";
    }
}