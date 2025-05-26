using System;
using System.Collections.Generic;
using System.Linq;

namespace lab3;

/// <summary>
/// Основной класс логгера, который объединяет фильтры и обработчики
/// Использует композицию для гибкой настройки системы логирования
/// </summary>
public class Logger
{
    private readonly List<ILogFilter> _filters;
    private readonly List<ILogHandler> _handlers;

    /// <summary>
    /// Создает новый экземпляр логгера
    /// </summary>
    /// <param name="filters">Список фильтров для проверки сообщений</param>
    /// <param name="handlers">Список обработчиков для вывода сообщений</param>
    public Logger(IEnumerable<ILogFilter>? filters = null, IEnumerable<ILogHandler>? handlers = null)
    {
        _filters = filters?.ToList() ?? new List<ILogFilter>();
        _handlers = handlers?.ToList() ?? new List<ILogHandler>();
    }

    /// <summary>
    /// Добавляет новый фильтр к логгеру
    /// </summary>
    /// <param name="filter">Фильтр для добавления</param>
    public void AddFilter(ILogFilter filter)
    {
        if (filter == null)
            throw new ArgumentNullException(nameof(filter));
        
        _filters.Add(filter);
    }

    /// <summary>
    /// Добавляет новый обработчик к логгеру
    /// </summary>
    /// <param name="handler">Обработчик для добавления</param>
    public void AddHandler(ILogHandler handler)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));
        
        _handlers.Add(handler);
    }

    /// <summary>
    /// Удаляет фильтр из логгера
    /// </summary>
    /// <param name="filter">Фильтр для удаления</param>
    public bool RemoveFilter(ILogFilter filter)
    {
        return _filters.Remove(filter);
    }

    /// <summary>
    /// Удаляет обработчик из логгера
    /// </summary>
    /// <param name="handler">Обработчик для удаления</param>
    public bool RemoveHandler(ILogHandler handler)
    {
        return _handlers.Remove(handler);
    }

    /// <summary>
    /// Основной метод логирования
    /// Проверяет сообщение через все фильтры и отправляет всем обработчикам
    /// </summary>
    /// <param name="text">Текст сообщения для логирования</param>
    public void Log(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        // Если нет фильтров, считаем что сообщение проходит проверку
        bool passesFilters = _filters.Count == 0;

        // Проверяем сообщение через все фильтры
        // Если хотя бы один фильтр пропускает сообщение, оно обрабатывается
        if (_filters.Count > 0)
        {
            foreach (var filter in _filters)
            {
                try
                {
                    if (filter.Match(text))
                    {
                        passesFilters = true;
                        break; // Достаточно одного совпадения
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в фильтре {filter.GetType().Name}: {ex.Message}");
                    // Продолжаем проверку другими фильтрами
                }
            }
        }

        // Если сообщение прошло фильтрацию, отправляем его всем обработчикам
        if (passesFilters)
        {
            foreach (var handler in _handlers)
            {
                try
                {
                    handler.Handle(text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в обработчике {handler.GetType().Name}: {ex.Message}");
                    // Продолжаем обработку другими обработчиками
                }
            }
        }
    }

    /// <summary>
    /// Возвращает количество активных фильтров
    /// </summary>
    public int FilterCount => _filters.Count;

    /// <summary>
    /// Возвращает количество активных обработчиков
    /// </summary>
    public int HandlerCount => _handlers.Count;

    /// <summary>
    /// Очищает все фильтры
    /// </summary>
    public void ClearFilters()
    {
        _filters.Clear();
    }

    /// <summary>
    /// Очищает все обработчики
    /// </summary>
    public void ClearHandlers()
    {
        _handlers.Clear();
    }
}