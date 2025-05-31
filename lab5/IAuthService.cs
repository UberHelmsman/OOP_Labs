namespace lab5;


/// <summary>
/// Интерфейс сервиса авторизации пользователей
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Авторизует пользователя в системе
    /// </summary>
    /// <param name="user">Пользователь для авторизации</param>
    void SignIn(User user);

    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    /// <param name="user">Пользователь для выхода</param>
    void SignOut(User user);

    /// <summary>
    /// Проверяет, авторизован ли пользователь
    /// </summary>
    bool IsAuthorized { get; }

    /// <summary>
    /// Получает текущего авторизованного пользователя
    /// </summary>
    User? CurrentUser { get; }
}