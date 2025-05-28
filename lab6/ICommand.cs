using System;

namespace lab6;


/// <summary>
/// Интерфейс для всех команд (паттерн Command).
/// Определяет основные операции, которые должна поддерживать каждая команда.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Выполняет команду.
    /// </summary>
    void Execute();

    /// <summary>
    /// Отменяет выполненную команду (операция undo).
    /// </summary>
    void Undo();

    /// <summary>
    /// Получает описание команды для логирования.
    /// </summary>
    /// <returns>Строковое описание команды</returns>
    string GetDescription();
}