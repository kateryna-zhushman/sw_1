using Microsoft.Data.SqlClient;

namespace ReceivingSystem.ConsoleApp;

internal static class CrudMenu
{
    public static void Run(
        string title,
        Action list,
        Action find,
        Action create,
        Action update,
        Action delete)
    {
        while (true)
        {
            ConsoleIo.Header(title);
            Console.WriteLine("1. Показати всі        (Read)");
            Console.WriteLine("2. Знайти за Id        (Read)");
            Console.WriteLine("3. Додати              (Create)");
            Console.WriteLine("4. Змінити             (Update)");
            Console.WriteLine("5. Видалити            (Delete)");
            Console.WriteLine("0. Назад");

            var choice = ConsoleIo.ReadString("Ваш вибір");

            try
            {
                switch (choice)
                {
                    case "1": list(); break;
                    case "2": find(); break;
                    case "3": create(); break;
                    case "4": update(); break;
                    case "5": delete(); break;
                    case "0": return;
                    default: ConsoleIo.Warn("Невідомий пункт меню."); break;
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
               
                ConsoleIo.Error("Операцію відхилено: запис пов'язаний з іншими даними (порушення зв'язку між таблицями).");
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                ConsoleIo.Error("Операцію відхилено: такий запис уже існує (порушення унікальності).");
            }
            catch (SqlException ex)
            {
                ConsoleIo.Error($"Помилка бази даних: {ex.Message}");
            }
        }
    }
}
