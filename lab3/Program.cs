using System;
using static System.Console;
using System.Collections.Generic;

using lab3;


WriteLine("=== Демонстрация системы логирования ===\n");

// Создаем различные фильтры
SimpleLogFilter errorFilter = new SimpleLogFilter("ERROR", caseSensitive: false);
SimpleLogFilter warningFilter = new SimpleLogFilter("warning", caseSensitive: false);

// Создаем различные обработчики
ConsoleHandler consoleHandler = new ConsoleHandler(includeTimeStamp: true);
FileHandler fileHandler = new FileHandler("application.log");
// var socketHandler = new SocketHandler("localhost", 12345); // Закомментировано, так как сервера у меня нет и делать его я не хочу
LinuxSyslogHandler syslogHandler = new LinuxSyslogHandler("TestApp");


WriteLine("---------------------------------------------------------------");
WriteLine("Демонстрация логгера с фильтром ERROR и консольным выводом:");

// Создаем логгер с фильтром только для ошибок
Logger errorLogger = new Logger(
    filters: new List<ILogFilter> { errorFilter },
    handlers: new List<ILogHandler> { consoleHandler }
);

// Тестируем разные сообщения
errorLogger.Log("INFO: приложение запущено");
errorLogger.Log("ERROR: критическая ошибка");
errorLogger.Log("WARNING: предупреждение о низкой памяти");
errorLogger.Log("ERROR: не удалось подключиться к базе данных");


WriteLine("\n--------------------------------------------------");
WriteLine("Демонстрация логгера с несколькими фильтрами:");
WriteLine("Фильтры warning, error, оба нечувствительны к регистру");

// Создаем логгер с несколькими фильтрами
Logger multiFilterLogger = new Logger();
multiFilterLogger.AddFilter(errorFilter);
multiFilterLogger.AddFilter(warningFilter);
multiFilterLogger.AddHandler(consoleHandler);
multiFilterLogger.AddHandler(fileHandler);

multiFilterLogger.Log("INFO: обычная инфа");
multiFilterLogger.Log("ERROR: ошибка");
multiFilterLogger.Log("WARNING: варнинг");
multiFilterLogger.Log("DEBUG: отладочная инфа");


WriteLine("\n---------------------------------------------------");
WriteLine("Демонстрация логгера с регулярными выражениями:");
WriteLine("Регулярное выражение на дату в начале сообщения");

RegexLogFilter regexFilter = new RegexLogFilter(@"^\d{4}-\d{2}-\d{2}"); // Сообщения, начинающиеся с даты
Logger regexLogger = new Logger(
    filters: new List<ILogFilter> { regexFilter },
    handlers: new List<ILogHandler> { consoleHandler }
);

regexLogger.Log("2025-06-25 сообщение с датой в начале");
regexLogger.Log("сообщение без даты");
regexLogger.Log("5312-12-31 еще одно сообщение с датой");


WriteLine("\n------------------------------------------------------");
WriteLine("Демонстрация логгера без фильтров (пропуск всех сообщений):");

Logger allMessagesLogger = new Logger(
    handlers: new List<ILogHandler> { consoleHandler }
);

allMessagesLogger.Log("сообщ1 выавапвапва");
allMessagesLogger.Log("сообще2 апрапрапрап");
allMessagesLogger.Log("ERROR: сообщение3 вапвапва с ошибкой");