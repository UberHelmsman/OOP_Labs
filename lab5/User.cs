namespace lab5;


/// <summary>
/// Представляет пользователя системы
/// </summary>
/// <param name="Id">Уникальный идентификатор пользователя</param>
/// <param name="Name">Имя пользователя</param>
/// <param name="Login">Логин для входа в систему</param>
/// <param name="Password">Пароль пользователя</param>
/// <param name="Email">Email адрес (необязательное поле)</param>
/// <param name="Address">Адрес пользователя (необязательное поле)</param>
public record User(
    int Id,
    string Name,
    string Login,
    string Password,
    string? Email = null,
    string? Address = null
) : IComparable<User>
{
    /// <summary>
    /// Реализует сортировку пользователей по имени
    /// </summary>
    /// <param name="other">Другой пользователь для сравнения</param>
    /// <returns>Результат сравнения имен</returns>
    public int CompareTo(User? other)
    {
        if (other == null) return 1;
        return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Переопределяет строковое представление, скрывая пароль
    /// </summary>
    /// <returns>Строковое представление пользователя без пароля</returns>
    public override string ToString()
    {
        return $"User {{ Id = {Id}, Name = {Name}, Login = {Login}, Email = {Email ?? "N/A"}, Address = {Address ?? "N/A"} }}";
    }
}