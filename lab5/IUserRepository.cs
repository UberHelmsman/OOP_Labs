namespace lab5;


/// <summary>
/// Интерфейс репозитория для работы с пользователями
/// Расширяет базовый репозиторий дополнительными методами для работы с User
/// </summary>
public interface IUserRepository : IDataRepository<User>
{
    /// <summary>
    /// Получает пользователя по логину
    /// </summary>
    /// <param name="login">Логин пользователя</param>
    /// <returns>Пользователь с указанным логином или null, если не найден</returns>
    User? GetByLogin(string login);
}