namespace ReceivingSystem.Dal;
public static class DbSettings
{
    public const string EnvironmentVariable = "RECEIVING_DB_CONNECTION";

    public const string DefaultConnectionString =
        "Server=DESKTOP-L7FKBHS;Database=receiving_db;Trusted_Connection=True;TrustServerCertificate=True;";

    public static string ConnectionString =>
        Environment.GetEnvironmentVariable(EnvironmentVariable) is { Length: > 0 } fromEnv
            ? fromEnv
            : DefaultConnectionString;
}
