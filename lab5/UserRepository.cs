namespace lab5;


/// <summary>
/// Реализация репозитория пользователей
/// Наследует от базового репозитория и добавляет специфичные для User методы
/// </summary>
public class UserRepository : DataRepository<User>, IUserRepository
{
    /// <summary>
    /// Инициализирует новый экземпляр репозитория пользователей
    /// </summary>
    public UserRepository() : base("users.json")
    {
    }

    /// <summary>
    /// Получает пользователя по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователь с указанным ID или null</returns>
    public override User? GetById(int id)
    {
        return GetAll().FirstOrDefault(u => u.Id == id);
    }

    /// <summary>
    /// Получает пользователя по логину
    /// </summary>
    /// <param name="login">Логин пользователя</param>
    /// <returns>Пользователь с указанным логином или null</returns>
    public User? GetByLogin(string login)
    {
        if (string.IsNullOrEmpty(login)) return null;

        return GetAll().FirstOrDefault(u =>
            string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Обновляет данные пользователя
    /// </summary>
    /// <param name="item">Пользователь для обновления</param>
    public override void Update(User item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        var existingUser = GetById(item.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"Пользователь с ID {item.Id} не найден");
        }


        var allUsers = GetAll().ToList();
        var index = allUsers.FindIndex(u => u.Id == item.Id);
        if (index >= 0)
        {
            Delete(existingUser);
            Add(item);
        }
    }
}