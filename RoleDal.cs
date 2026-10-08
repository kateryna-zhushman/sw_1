using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Sql;

public class RoleDal : DalBase, IRoleDal
{
    private const string Select = "SELECT id, name, page_url FROM roles";

    public RoleDal(string connectionString) : base(connectionString) { }

    private static Role Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Name = Str(r, 1),
        PageUrl = Str(r, 2)
    };

    public int Create(Role role) =>
        Insert("INSERT INTO roles (name, page_url) VALUES (@name, @page_url)",
            P("@name", role.Name),
            P("@page_url", role.PageUrl));

    public Role? GetById(int id) =>
        QuerySingle(Select + " WHERE id = @id", Map, P("@id", id));

    public IReadOnlyList<Role> GetAll() =>
        Query(Select + " ORDER BY id", Map);

    public bool Update(Role role) =>
        Execute("UPDATE roles SET name = @name, page_url = @page_url WHERE id = @id",
            P("@name", role.Name),
            P("@page_url", role.PageUrl),
            P("@id", role.Id)) > 0;

    public bool Delete(int id) =>
        Execute("DELETE FROM roles WHERE id = @id", P("@id", id)) > 0;
}
