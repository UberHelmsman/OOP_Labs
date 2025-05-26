using System;
using System.Text.RegularExpressions;

namespace lab3;

/// <summary>
/// Простой фильтр, который проверяет вхождение текстового паттерна в сообщение
/// </summary>
public class SimpleLogFilter : ILogFilter
{
    private readonly string _pattern;
    private readonly bool _caseSensitive;

    /// <summary>
    /// Создает новый экземпляр простого фильтра
    /// </summary>
    /// <param name="pattern">Паттерн для поиска</param>
    /// <param name="caseSensitive">Учитывать ли регистр (по умолчанию - нет)</param>
    public SimpleLogFilter(string pattern, bool caseSensitive = false)
    {
        _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        _caseSensitive = caseSensitive;
    }

    /// <summary>
    /// Проверяет, содержит ли текст заданный паттерн
    /// </summary>
    public bool Match(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        var comparison = _caseSensitive 
            ? StringComparison.Ordinal 
            : StringComparison.OrdinalIgnoreCase;
        
        return text.Contains(_pattern, comparison);
    }
}

/// <summary>
/// Фильтр на основе регулярных выражений
/// </summary>
public class RegexLogFilter : ILogFilter
{
    private readonly Regex _regex;

    /// <summary>
    /// Создает новый экземпляр фильтра на основе регулярного выражения
    /// </summary>
    /// <param name="pattern">Паттерн регулярного выражения</param>
    /// <param name="ignoreCase">Игнорировать регистр (по умолчанию - да)</param>
    public RegexLogFilter(string pattern, bool ignoreCase = true)
    {
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException("Pattern cannot be null or empty", nameof(pattern));

        var options = RegexOptions.Compiled;
        if (ignoreCase)
            options |= RegexOptions.IgnoreCase;

        _regex = new Regex(pattern, options);
    }

    /// <summary>
    /// Проверяет, соответствует ли текст регулярному выражению
    /// </summary>
    public bool Match(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        return _regex.IsMatch(text);
    }
}
