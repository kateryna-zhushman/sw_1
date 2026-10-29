using System.Text;
using Microsoft.Data.SqlClient;
using ReceivingSystem.ConsoleApp;
using ReceivingSystem.Dal;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Sql;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var connectionString = DbSettings.ConnectionString;
IRoleDal roles = new RoleDal(connectionString);
ICategoryDal categories = new CategoryDal(connectionString);
IUserDal users = new UserDal(connectionString);
IProductDal products = new ProductDal(connectionString);
IReceiptDal receipts = new ReceiptDal(connectionString);

ConsoleIo.Header("Демонстрація DAL: приймання товару");

try
{
    var roleCount = roles.GetAll().Count;
    ConsoleIo.Info($"Підключено до бази даних. Ролей у таблиці roles: {roleCount}");
}
catch (SqlException ex)
{
    ConsoleIo.Error("Не вдалося підключитися до бази даних:");
    ConsoleIo.Error(ex.Message);
    Console.WriteLine();
    Console.WriteLine("Перевірте рядок підключення в ReceivingSystem.Dal/DbSettings.cs");
    Console.WriteLine($"або задайте змінну середовища {DbSettings.EnvironmentVariable}.");
    Console.WriteLine("Натисніть Enter для виходу...");
    Console.ReadLine();
    return;
}

var menus = new Menus(roles, categories, users, products, receipts);
var demo = new AutoDemo(roles, categories, users, products, receipts);

while (true)
{
    ConsoleIo.Header("Головне меню");
    Console.WriteLine("1. Ролі");
    Console.WriteLine("2. Категорії");
    Console.WriteLine("3. Користувачі");
    Console.WriteLine("4. Товари");
    Console.WriteLine("5. Приймання товару");
    Console.WriteLine("6. Автоматична демонстрація CRUD");
    Console.WriteLine("0. Вихід");

    var choice = ConsoleIo.ReadString("Ваш вибір");

    try
    {
        switch (choice)
        {
            case "1": menus.Roles(); break;
            case "2": menus.Categories(); break;
            case "3": menus.Users(); break;
            case "4": menus.Products(); break;
            case "5": menus.Receipts(); break;
            case "6": demo.Run(); break;
            case "0": return;
            default: ConsoleIo.Warn("Невідомий пункт меню."); break;
        }
    }
    catch (SqlException ex)
    {
        ConsoleIo.Error($"Помилка бази даних: {ex.Message}");
    }
}
