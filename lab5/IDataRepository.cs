namespace lab5;


/// <summary>
/// Базовый интерфейс репозитория для CRUD операций с произвольным типом данных
/// </summary>
/// <typeparam name="T">Тип данных для работы с репозиторием</typeparam>
public interface IDataRepository<T>
{
    /// <summary>
    /// Получает все элементы из репозитория
    /// </summary>
    /// <returns>Коллекция всех элементов</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Получает элемент по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор элемента</param>
    /// <returns>Элемент с указанным идентификатором или null, если не найден</returns>
    T? GetById(int id);

    /// <summary>
    /// Добавляет новый элемент в репозиторий
    /// </summary>
    /// <param name="item">Элемент для добавления</param>
    void Add(T item);

    /// <summary>
    /// Обновляет существующий элемент в репозитории
    /// </summary>
    /// <param name="item">Элемент для обновления</param>
    void Update(T item);

    /// <summary>
    /// Удаляет элемент из репозитория
    /// </summary>
    /// <param name="item">Элемент для удаления</param>
    void Delete(T item);
}