using System;

namespace lab3;

/// <summary>
/// Интерфейс для обработчиков логов
/// Определяет контракт для различных способов вывода лог-сообщений
/// </summary>
public interface ILogHandler
{
    /// <summary>
    /// Обрабатывает (выводит/сохраняет) лог-сообщение
    /// </summary>
    /// <param name="text">Текст сообщения для обработки</param>
    void Handle(string text);
}