namespace lab4;


/// <summary>
/// Интерфейс для слушателей изменений свойств
/// </summary>
/// <typeparam name="T">Тип объекта, изменения которого отслеживаются</typeparam>
public interface IPropertyChangedListener<T>
{
    /// <summary>
    /// Вызывается когда свойство объекта изменилось
    /// </summary>
    /// <param name="obj">Объект, свойство которого изменилось</param>
    /// <param name="propertyName">Имя изменившегося свойства</param>
    void OnPropertyChanged(T obj, string propertyName);
}