using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Sql;

public class UserDal : DalBase, IUserDal
{
    private const string Select = "SELECT id, username, password_hash, role_id FROM users";

    public UserDal(string connectionString) : base(connectionString) { }

    private static User Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Username = Str(r, 1),
        PasswordHash = Str(r, 2),
        RoleId = r.GetInt32(3)
    };

    public int Create(User user) =>
        Insert("INSERT INTO users (username, password_hash, role_id) VALUES (@username, @password_hash, @role_id)",
            P("@username", user.Username),
            P("@password_hash", user.PasswordHash),
            P("@role_id", user.RoleId));

    public User? GetById(int id) =>
        QuerySingle(Select + " WHERE id = @id", Map, P("@id", id));

    public IReadOnlyList<User> GetAll() =>
        Query(Select + " ORDER BY id", Map);

    public bool Update(User user) =>
        Execute("UPDATE users SET username = @username, password_hash = @password_hash, role_id = @role_id WHERE id = @id",
            P("@username", user.Username),
            P("@password_hash", user.PasswordHash),
            P("@role_id", user.RoleId),
            P("@id", user.Id)) > 0;

    public bool Delete(int id) =>
        Execute("DELETE FROM users WHERE id = @id", P("@id", id)) > 0;

    public User? GetByUsername(string username) =>
        QuerySingle(Select + " WHERE username = @username", Map, P("@username", username));
}
