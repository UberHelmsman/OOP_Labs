namespace lab4;


/// <summary>
/// Интерфейс для валидаторов изменений свойств
/// </summary>
/// <typeparam name="T">Тип объекта, изменения которого валидируются</typeparam>
public interface IPropertyChangingListener<T>
{
    /// <summary>
    /// Вызывается перед изменением свойства для валидации
    /// </summary>
    /// <param name="obj">Объект, свойство которого изменяется</param>
    /// <param name="propertyName">Имя изменяемого свойства</param>
    /// <param name="oldValue">Старое значение</param>
    /// <param name="newValue">Новое значение</param>
    /// <returns>true если изменение разрешено, false если запрещено</returns>
    bool OnPropertyChanging(T obj, string propertyName, object oldValue, object newValue);
}