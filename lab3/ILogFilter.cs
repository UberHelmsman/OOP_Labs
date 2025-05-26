namespace lab3;

/// <summary>
/// Интерфейс для фильтров логов
/// Определяет контракт для проверки, должно ли сообщение быть обработано
/// </summary>
public interface ILogFilter
{
    /// <summary>
    /// Проверяет, соответствует ли текст условиям фильтра
    /// </summary>
    /// <param name="text">Текст для проверки</param>
    /// <returns>true, если текст проходит фильтр; false - если не проходит</returns>
    bool Match(string text);
}
