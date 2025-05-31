using System;
using System.Collections.Generic;

namespace lab4;


/// <summary>
/// Класс Person который поддерживает уведомления об изменениях и валидацию
/// </summary>
public class Person : INotifyDataChanged<Person>, INotifyDataChanging<Person>
{
    // Поля для хранения данных
    private string _name;
    private int _age;
    private string _email;

    // Списки слушателей
    private readonly List<IPropertyChangedListener<Person>> _changedListeners = new List<IPropertyChangedListener<Person>>();
    private readonly List<IPropertyChangingListener<Person>> _changingListeners = new List<IPropertyChangingListener<Person>>();

    /// <summary>
    /// Имя человека
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, nameof(Name));
    }

    /// <summary>
    /// Возраст человека
    /// </summary>
    public int Age
    {
        get => _age;
        set => SetProperty(ref _age, value, nameof(Age));
    }

    /// <summary>
    /// Email человека
    /// </summary>
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value, nameof(Email));
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="age">Возраст</param>
    /// <param name="email">Email</param>
    public Person(string name = "", int age = 0, string email = "")
    {
        _name = name;
        _age = age;
        _email = email;
    }

    /// <summary>
    /// Универсальный метод для установки свойств с валидацией и уведомлениями
    /// </summary>
    /// <typeparam name="T">Тип свойства</typeparam>
    /// <param name="field">Ссылка на поле</param>
    /// <param name="value">Новое значение</param>
    /// <param name="propertyName">Имя свойства</param>
    private void SetProperty<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        // Валидация изменения
        if (!NotifyPropertyChanging(propertyName, field, value))
            return; // Валидация не прошла, изменение отклонено

        // Изменяем значение
        field = value;

        // Уведомляем об изменении
        NotifyPropertyChanged(propertyName);
    }

    /// <summary>
    /// Уведомляет валидаторов о предстоящем изменении
    /// </summary>
    /// <param name="propertyName">Имя свойства</param>
    /// <param name="oldValue">Старое значение</param>
    /// <param name="newValue">Новое значение</param>
    /// <returns>true если изменение разрешено</returns>
    private bool NotifyPropertyChanging(string propertyName, object oldValue, object newValue)
    {
        foreach (var validator in _changingListeners)
        {
            if (!validator.OnPropertyChanging(this, propertyName, oldValue, newValue))
                return false; // Один из валидаторов отклонил изменение
        }
        return true;
    }

    /// <summary>
    /// Уведомляет слушателей об изменении
    /// </summary>
    /// <param name="propertyName">Имя изменившегося свойства</param>
    private void NotifyPropertyChanged(string propertyName)
    {
        foreach (var listener in _changedListeners)
        {
            listener.OnPropertyChanged(this, propertyName);
        }
    }

    // Реализация INotifyDataChanged
    public void AddPropertyChangedListener(IPropertyChangedListener<Person> listener)
    {
        if (!_changedListeners.Contains(listener))
            _changedListeners.Add(listener);
    }

    public void RemovePropertyChangedListener(IPropertyChangedListener<Person> listener)
    {
        _changedListeners.Remove(listener);
    }

    // Реализация INotifyDataChanging
    public void AddPropertyChangingListener(IPropertyChangingListener<Person> listener)
    {
        if (!_changingListeners.Contains(listener))
            _changingListeners.Add(listener);
    }

    public void RemovePropertyChangingListener(IPropertyChangingListener<Person> listener)
    {
        _changingListeners.Remove(listener);
    }

    /// <summary>
    /// Переопределение ToString для удобного вывода
    /// </summary>
    /// <returns>Строковое представление объекта</returns>
    public override string ToString()
    {
        return $"Person: Name={Name}, Age={Age}, Email={Email}";
    }
}