using System.Text.Json;

namespace lab5;


/// <summary>
/// Реализация репозитория данных с хранением в файле
/// Использует JSON для сериализации данных
/// </summary>
/// <typeparam name="T">Тип данных для хранения</typeparam>
public class DataRepository<T> : IDataRepository<T> where T : class
{
    private readonly string _filePath;
    private readonly List<T> _items;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория
    /// </summary>
    /// <param name="fileName">Имя файла для хранения данных</param>
    public DataRepository(string fileName)
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        _items = new List<T>();
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        LoadData();
    }

    /// <summary>
    /// Загружает данные из файла
    /// </summary>
    private void LoadData()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    var items = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
                    if (items != null)
                    {
                        _items.Clear();
                        _items.AddRange(items);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
        }
    }

    /// <summary>
    /// Сохраняет данные в файл
    /// </summary>
    private void SaveData()
    {
        try
        {
            var json = JsonSerializer.Serialize(_items, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
        }
    }

    /// <summary>
    /// Получает все элементы из репозитория
    /// </summary>
    /// <returns>Коллекция всех элементов</returns>
    public IEnumerable<T> GetAll()
    {
        return _items.AsReadOnly();
    }

    /// <summary>
    /// Получает элемент по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор элемента</param>
    /// <returns>Элемент с указанным идентификатором или null</returns>
    public virtual T? GetById(int id)
    {
        // Базовая реализация - нужно переопределить в наследниках
        return null;
    }

    /// <summary>
    /// Добавляет новый элемент в репозиторий
    /// </summary>
    /// <param name="item">Элемент для добавления</param>
    public void Add(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Add(item);
        SaveData();
    }

    /// <summary>
    /// Обновляет существующий элемент в репозитории
    /// </summary>
    /// <param name="item">Элемент для обновления</param>
    public virtual void Update(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        // Базовая реализация - сохраняем изменения
        SaveData();
    }

    /// <summary>
    /// Удаляет элемент из репозитория
    /// </summary>
    /// <param name="item">Элемент для удаления</param>
    public void Delete(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Remove(item);
        SaveData();
    }
}