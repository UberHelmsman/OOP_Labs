using System;

namespace lab4;


/// <summary>
/// Простой слушатель который логирует все изменения
/// </summary>
public class SimpleLogListener : IPropertyChangedListener<Person>
{
    private readonly string _name;

    public SimpleLogListener(string name)
    {
        _name = name;
    }

    public void OnPropertyChanged(Person obj, string propertyName)
    {
        Console.WriteLine($"[{_name}] Изменилось свойство {propertyName} у {obj}");
    }
}

/// <summary>
/// Слушатель который считает количество изменений
/// </summary>
public class ChangeCounterListener : IPropertyChangedListener<Person>
{
    public int ChangeCount { get; private set; } = 0;

    public void OnPropertyChanged(Person obj, string propertyName)
    {
        ChangeCount++;
        Console.WriteLine($"[Счетчик] Общее количество изменений: {ChangeCount}");
    }
}

/// <summary>
/// Валидатор возраста - проверяет что возраст положительный и разумный
/// </summary>
public class AgeValidator : IPropertyChangingListener<Person>
{
    public bool OnPropertyChanging(Person obj, string propertyName, object oldValue, object newValue)
    {
        if (propertyName == "Age" && newValue is int age)
        {
            if (age < 0 || age > 150)
            {
                Console.WriteLine($"[Валидатор возраста] ОТКЛОНЕНО: Возраст {age} недопустим (должен быть от 0 до 150)");
                return false;
            }
            Console.WriteLine($"[Валидатор возраста] OK: Возраст {age} валиден");
        }
        return true;
    }
}

/// <summary>
/// Валидатор email - проверяет что email содержит @
/// </summary>
public class EmailValidator : IPropertyChangingListener<Person>
{
    public bool OnPropertyChanging(Person obj, string propertyName, object oldValue, object newValue)
    {
        if (propertyName == "Email" && newValue is string email)
        {
            if (!string.IsNullOrEmpty(email) && !email.Contains("@"))
            {
                Console.WriteLine($"[Валидатор Email] ОТКЛОНЕНО: Email '{email}' должен содержать @");
                return false;
            }
            Console.WriteLine($"[Валидатор Email] OK: Email '{email}' валиден");
        }
        return true;
    }
}

/// <summary>
/// Валидатор имени - проверяет что имя не пустое
/// </summary>
public class NameValidator : IPropertyChangingListener<Person>
{
    public bool OnPropertyChanging(Person obj, string propertyName, object oldValue, object newValue)
    {
        if (propertyName == "Name" && newValue is string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine($"[Валидатор имени] ОТКЛОНЕНО: Имя не может быть пустым");
                return false;
            }
            Console.WriteLine($"[Валидатор имени] OK: Имя '{name}' валидно");
        }
        return true;
    }
}