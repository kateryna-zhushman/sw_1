using Microsoft.Data.SqlClient;

namespace ReceivingSystem.Dal.Sql;

public abstract class DalBase
{
    private readonly string _connectionString;

    protected DalBase(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Рядок підключення не може бути порожнім.", nameof(connectionString));

        _connectionString = connectionString;
    }

    protected static SqlParameter P(string name, object? value) =>
        new SqlParameter(name, value ?? DBNull.Value);

    protected static string Str(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);

    protected int Insert(string insertSql, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(insertSql + "; SELECT CAST(SCOPE_IDENTITY() AS int);", connection);
        command.Parameters.AddRange(parameters);
        connection.Open();
        return (int)command.ExecuteScalar()!;
    }
    protected int Execute(string sql, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
        connection.Open();
        return command.ExecuteNonQuery();
    }

    protected List<T> Query<T>(string sql, Func<SqlDataReader, T> map, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
        connection.Open();
        using var reader = command.ExecuteReader();

        var result = new List<T>();
        while (reader.Read())
            result.Add(map(reader));
        return result;
    }

    protected T? QuerySingle<T>(string sql, Func<SqlDataReader, T> map, params SqlParameter[] parameters)
        where T : class
    {
        return Query(sql, map, parameters).FirstOrDefault();
    }
}
