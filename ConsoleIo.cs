namespace ReceivingSystem.ConsoleApp;

internal static class ConsoleIo
{
    public static string ReadString(string prompt, string? current = null)
    {
        while (true)
        {
            Console.Write(current is null ? $"{prompt}: " : $"{prompt} [{current}]: ");
            var input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(input))
                return input;
            if (current is not null)
                return current;

            Warn("Значення не може бути порожнім.");
        }
    }

    public static int ReadInt(string prompt, int? current = null)
    {
        while (true)
        {
            Console.Write(current is null ? $"{prompt}: " : $"{prompt} [{current}]: ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input) && current is not null)
                return current.Value;
            if (int.TryParse(input, out var value))
                return value;

            Warn("Введіть ціле число.");
        }
    }

    public static int ReadPositiveInt(string prompt, int? current = null)
    {
        while (true)
        {
            var value = ReadInt(prompt, current);
            if (value > 0)
                return value;

            Warn("Число має бути більше нуля.");
        }
    }

    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Header(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"=== {title} ===");
        Console.ResetColor();
    }
}
