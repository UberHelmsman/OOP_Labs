using System;
using System.Text;

namespace lab4;


/// <summary>
/// Команда для печати символа в текстовый буфер.
/// Реализует паттерн Command для операций с текстом.
/// </summary>
public class PrintCharacterCommand : ICommand
{
    /// <summary>
    /// Символ, который нужно напечатать
    /// </summary>
    private char _char;

    /// <summary>
    /// Ссылка на текстовый буфер, в который производится печать
    /// </summary>
    private StringBuilder _textBuffer;

    /// <summary>
    /// Инициализирует новый экземпляр команды печати символа.
    /// </summary>
    /// <param name="character">Символ для печати</param>
    /// <param name="textBuffer">Текстовый буфер для вывода</param>
    public PrintCharacterCommand(char character, StringBuilder textBuffer)
    {
        _char = character;
        _textBuffer = textBuffer;
    }

    /// <summary>
    /// Выполняет команду печати символа - добавляет символ в конец текстового буфера.
    /// </summary>
    public void Execute()
    {
        _textBuffer.Append(_char);
    }

    /// <summary>
    /// Отменяет команду печати символа - удаляет последний символ из текстового буфера.
    /// </summary>
    public void Undo()
    {
        // Удаляем последний символ
        if (_textBuffer.Length > 0)
        {
            _textBuffer.Length--;
        }
    }

    /// <summary>
    /// Получает описание команды для логирования.
    /// </summary>
    /// <returns>Напечатанный символ в виде строки</returns>
    public string GetDescription()
    {
        return _char.ToString();
    }
}