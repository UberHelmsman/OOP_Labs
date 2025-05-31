using System;

using lab4;



var person = new Person("Иван", 25, "ivan@example.com");
Console.WriteLine($"Создан объект: {person}\n");


var logListener1 = new SimpleLogListener("Логгер-1");
var logListener2 = new SimpleLogListener("Логгер-2");
var counterListener = new ChangeCounterListener();


var ageValidator = new AgeValidator();
var emailValidator = new EmailValidator();
var nameValidator = new NameValidator();


person.AddPropertyChangedListener(logListener1);
person.AddPropertyChangedListener(logListener2);
person.AddPropertyChangedListener(counterListener);


person.AddPropertyChangingListener(ageValidator);
person.AddPropertyChangingListener(emailValidator);
person.AddPropertyChangingListener(nameValidator);



Console.WriteLine("тест1, валидация изменений");
person.Name = "Петр";
person.Age = 30;
person.Email = "petr@gmail.com";
Console.WriteLine($"Результат: {person}\n");

Console.WriteLine("тест2, невалидные изменения");
person.Age = -5; 
person.Age = 200;

person.Email = "бебебебеэмейлбезсобаки"; 

person.Name = ""; 
person.Name = "   "; 

Console.WriteLine($"\nперсон после невалидных попыток: {person}");
Console.WriteLine($"общее колво успешных изменений: {counterListener.ChangeCount}\n");