using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Sql;

public class CategoryDal : DalBase, ICategoryDal
{
    private const string Select = "SELECT id, name FROM categories";

    public CategoryDal(string connectionString) : base(connectionString) { }

    private static Category Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Name = Str(r, 1)
    };

    public int Create(Category category) =>
        Insert("INSERT INTO categories (name) VALUES (@name)",
            P("@name", category.Name));

    public Category? GetById(int id) =>
        QuerySingle(Select + " WHERE id = @id", Map, P("@id", id));

    public IReadOnlyList<Category> GetAll() =>
        Query(Select + " ORDER BY id", Map);

    public bool Update(Category category) =>
        Execute("UPDATE categories SET name = @name WHERE id = @id",
            P("@name", category.Name),
            P("@id", category.Id)) > 0;

    public bool Delete(int id) =>
        Execute("DELETE FROM categories WHERE id = @id", P("@id", id)) > 0;
}
