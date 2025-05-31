namespace lab4;


/// <summary>
/// Интерфейс для классов, которые позволяют валидировать изменения своих данных
/// </summary>
/// <typeparam name="T">Тип объекта</typeparam>
public interface INotifyDataChanging<T>
{
    /// <summary>
    /// Добавляет валидатор изменений свойств
    /// </summary>
    /// <param name="listener">Валидатор для добавления</param>
    void AddPropertyChangingListener(IPropertyChangingListener<T> listener);

    /// <summary>
    /// Удаляет валидатор изменений свойств
    /// </summary>
    /// <param name="listener">Валидатор для удаления</param>
    void RemovePropertyChangingListener(IPropertyChangingListener<T> listener);
}