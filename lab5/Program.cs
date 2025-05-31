using lab5;


var userRepo = new UserRepository();
var authService = new AuthService();

var user1 = new User(1, "Иван Иванов", "ivan", "123", "ivan@mail.ru", "Москва");
var user2 = new User(2, "Петр Петров", "petr", "456", "petr@mail.ru", "СПб");
var user3 = new User(3, "Анна Сидорова", "anna", "789", null, null);

userRepo.Add(user1);
userRepo.Add(user2);
userRepo.Add(user3);


if (!authService.IsAuthorized)
{
    var loginUser = userRepo.GetById(1);
    if (loginUser != null)
    {
        authService.SignIn(loginUser);
    }
}
else
{
    Console.WriteLine($"Уже авторизован: {authService.CurrentUser?.Name}");
}


var annaUser = userRepo.GetById(3);
if (annaUser != null)
{
    var updatedAnna = annaUser with { Email = "anna@updated.com" };
    userRepo.Update(updatedAnna);
}


if (authService.CurrentUser != null)
{
    authService.SignOut(authService.CurrentUser);
}

var petrUser = userRepo.GetById(2);
if (petrUser != null)
{
    authService.SignIn(petrUser);
}
