using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace lab3;

/// <summary>
/// Обработчик для записи логов в файл
/// </summary>
public class FileHandler : ILogHandler
{
    private readonly string _filePath;



    /// <summary>
    /// СОздает экземпляр файлового обработчика
    /// </summary>
    /// <param name = "filePath"> Путь к файлу для записи логов </param>
    public FileHandler(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    /// <summary>
    /// Записывает сообщение в файл с отметкой времени
    /// </summary>
    /// <param name= "text"> Сообщение для записи в файл </param>



    public void Handle(string text)
    {
        try
        {
            var logEntry = $"{DateTime.Now:yyyy-NN-dd HH:mm:ss} - {text}\n";
            File.AppendAllLines(_filePath, [logEntry]);
        }
        catch (Exception ex)
        {

            System.Console.WriteLine($"Ошибка записи в файл: {ex.Message}");
        }
    }
}

/// <summary> Обработчик для вывода логов в консоль </summary>
public class ConsoleHandler : ILogHandler
{
    private readonly bool _inclueTimeStamp;


    /// <summary> Создает новый экземпляр консольного обработчика </summary>
    /// <param name="_includeTimeStamp"> Включать ли временную метку</param>
    public ConsoleHandler(bool includeTimeStamp = true)
    {
        _inclueTimeStamp = includeTimeStamp;
    }

    /// <summary> Выводит сообщение в консоль </summary>
    public void Handle(string text)
    {
        var output = _inclueTimeStamp ? $"[{DateTime.Now:HH:mm:ss}] {text}" : text;
        System.Console.WriteLine(output);
    }
}


/// <summary> Обработчик для отправки логов через сокет </summary>
public class SocketHandler : ILogHandler
{
    private readonly string _host;
    private readonly int _port;

    /// <summary> Создает новый экземпляр сокетного хендлера </summary>
    /// <param name="host"> АДрес хоста </param>
    /// <param name="port"> Порт </param>
    public SocketHandler(string host, int port)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _port = port;
    }


    /// <summary> Отправляет сообщение через tcp сокет </summary>
    public void Handle(string text)
    {
        try
        {
            using (var client = new TcpClient())
            {
                var connectTask = client.ConnectAsync(_host, _port);
                if (connectTask.Wait(5000))
                {
                    var data = Encoding.UTF8.GetBytes($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {text}\n");
                    var stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                else
                {
                    System.Console.WriteLine($"Не удалось подключиться к {_host}:{_port}");
                }

            }
        }
        catch (Exception ex)
        {

            System.Console.WriteLine($"Ошибка отправки через сокет: {ex.Message}");
        }
    }
}


/// <summary>
/// Простой обработчик для записи в системные логи Linux (syslog)
/// </summary>
public class LinuxSyslogHandler : ILogHandler
{
    private readonly string _appName;

    /// <summary>
    /// Создает новый экземпляр обработчика системных логов Linux
    /// </summary>
    /// <param name="appName">Имя приложения в логах</param>
    public LinuxSyslogHandler(string appName = "MyApp")
    {
        _appName = appName ?? "MyApp";
        
        try
        {
            // Открываем соединение с syslog
            openlog(_appName, 1, 16); // 1 = LOG_PID, 16 = LOG_USER
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации syslog: {ex.Message}");
        }
    }

    /// <summary>
    /// Записывает сообщение в системный журнал
    /// </summary>
    public void Handle(string text)
    {
        try
        {
            // Записываем в syslog с уровнем INFO (6)
            syslog(6, text);
        }
        catch (Exception ex)
        {
            // Если syslog не работает, выводим в консоль
            Console.WriteLine($"[SYSLOG] {DateTime.Now:HH:mm:ss} {_appName}: {text}");
            Console.WriteLine($"Ошибка syslog: {ex.Message}");
        }
    }

    // Системные функции Linux для работы с syslog
    [DllImport("libc")]
    private static extern void openlog(string ident, int option, int facility);

    [DllImport("libc")]
    private static extern void syslog(int priority, string message);
}