namespace lab4;


/// <summary>
/// Интерфейс для классов, которые уведомляют об изменениях своих данных
/// </summary>
/// <typeparam name="T">Тип объекта</typeparam>
public interface INotifyDataChanged<T>
{
    /// <summary>
    /// Добавляет слушателя изменений свойств
    /// </summary>
    /// <param name="listener">Слушатель для добавления</param>
    void AddPropertyChangedListener(IPropertyChangedListener<T> listener);

    /// <summary>
    /// Удаляет слушателя изменений свойств
    /// </summary>
    /// <param name="listener">Слушатель для удаления</param>
    void RemovePropertyChangedListener(IPropertyChangedListener<T> listener);
}