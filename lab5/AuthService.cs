using System.Text.Json;

namespace lab5;


/// <summary>
/// Реализация сервиса авторизации с автоматическим входом
/// Сохраняет информацию о текущем пользователе в файл для автоматической авторизации
/// </summary>
public class AuthService : IAuthService
{
    private readonly string _sessionFilePath;
    private readonly JsonSerializerOptions _jsonOptions;
    private User? _currentUser;

    /// <summary>
    /// Данные сессии для сохранения в файл
    /// </summary>
    private class SessionData
    {
        public int UserId { get; set; }
        public string Login { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
    }

    /// <summary>
    /// Инициализирует новый экземпляр сервиса авторизации
    /// </summary>
    public AuthService()
    {
        _sessionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.json");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Попытка автоматической авторизации при создании сервиса
        TryAutoLogin();
    }

    /// <summary>
    /// Проверяет, авторизован ли пользователь
    /// </summary>
    public bool IsAuthorized => _currentUser != null;

    /// <summary>
    /// Получает текущего авторизованного пользователя
    /// </summary>
    public User? CurrentUser => _currentUser;

    /// <summary>
    /// Авторизует пользователя в системе
    /// </summary>
    /// <param name="user">Пользователь для авторизации</param>
    public void SignIn(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        _currentUser = user;
        SaveSession();

        Console.WriteLine($"Пользователь {user.Name} ({user.Login}) успешно авторизован");
    }

    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    /// <param name="user">Пользователь для выхода</param>
    public void SignOut(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (_currentUser?.Id == user.Id)
        {
            Console.WriteLine($"Пользователь {_currentUser.Name} вышел из системы");
            _currentUser = null;
            ClearSession();
        }
        else
        {
            Console.WriteLine("Попытка выхода пользователя, который не авторизован");
        }
    }

    /// <summary>
    /// Сохраняет информацию о сессии в файл
    /// </summary>
    private void SaveSession()
    {
        if (_currentUser == null) return;

        try
        {
            var sessionData = new SessionData
            {
                UserId = _currentUser.Id,
                Login = _currentUser.Login,
                LoginTime = DateTime.Now
            };

            var json = JsonSerializer.Serialize(sessionData, _jsonOptions);
            File.WriteAllText(_sessionFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении сессии: {ex.Message}");
        }
    }

    /// <summary>
    /// Очищает информацию о сессии
    /// </summary>
    private void ClearSession()
    {
        try
        {
            if (File.Exists(_sessionFilePath))
            {
                File.Delete(_sessionFilePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при очистке сессии: {ex.Message}");
        }
    }

    /// <summary>
    /// Попытка автоматического входа на основе сохраненной сессии
    /// </summary>
    private void TryAutoLogin()
    {
        try
        {
            if (!File.Exists(_sessionFilePath)) return;

            var json = File.ReadAllText(_sessionFilePath);
            if (string.IsNullOrEmpty(json)) return;

            var sessionData = JsonSerializer.Deserialize<SessionData>(json, _jsonOptions);
            if (sessionData == null) return;

            if (DateTime.Now.Subtract(sessionData.LoginTime).TotalDays > 7)
            {
                ClearSession();
                return;
            }

            Console.WriteLine($"Найдена активная сессия для пользователя: {sessionData.Login}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при автоматическом входе: {ex.Message}");
            ClearSession();
        }
    }

    /// <summary>
    /// Восстанавливает сессию пользователя из репозитория
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователей</param>
    public void TryRestoreSession(IUserRepository userRepository)
    {
        if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

        try
        {
            if (!File.Exists(_sessionFilePath)) return;

            var json = File.ReadAllText(_sessionFilePath);
            if (string.IsNullOrEmpty(json)) return;

            var sessionData = JsonSerializer.Deserialize<SessionData>(json, _jsonOptions);
            if (sessionData == null) return;

            // Проверяем, не истекла ли сессия
            if (DateTime.Now.Subtract(sessionData.LoginTime).TotalDays > 7)
            {
                ClearSession();
                return;
            }

            // Восстанавливаем пользователя из репозитория
            var user = userRepository.GetById(sessionData.UserId);
            if (user != null)
            {
                _currentUser = user;
                Console.WriteLine($"Автоматическая авторизация: {user.Name} ({user.Login})");
            }
            else
            {
                // Пользователь не найден, очищаем сессию
                ClearSession();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при восстановлении сессии: {ex.Message}");
            ClearSession();
        }
    }
}