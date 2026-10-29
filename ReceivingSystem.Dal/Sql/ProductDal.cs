using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Sql;

public class ProductDal : DalBase, IProductDal
{
    private const string Select = "SELECT id, name, category_id FROM products";

    public ProductDal(string connectionString) : base(connectionString) { }

    private static Product Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Name = Str(r, 1),
        CategoryId = r.GetInt32(2)
    };

    public int Create(Product product) =>
        Insert("INSERT INTO products (name, category_id) VALUES (@name, @category_id)",
            P("@name", product.Name),
            P("@category_id", product.CategoryId));

    public Product? GetById(int id) =>
        QuerySingle(Select + " WHERE id = @id", Map, P("@id", id));

    public IReadOnlyList<Product> GetAll() =>
        Query(Select + " ORDER BY id", Map);

    public bool Update(Product product) =>
        Execute("UPDATE products SET name = @name, category_id = @category_id WHERE id = @id",
            P("@name", product.Name),
            P("@category_id", product.CategoryId),
            P("@id", product.Id)) > 0;

    public bool Delete(int id) =>
        Execute("DELETE FROM products WHERE id = @id", P("@id", id)) > 0;

    public IReadOnlyList<Product> GetByCategory(int categoryId) =>
        Query(Select + " WHERE category_id = @category_id ORDER BY id", Map, P("@category_id", categoryId));
}
