using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Sql;

public class ReceiptDal : DalBase, IReceiptDal
{
    private const string Select = "SELECT id, product_id, user_id, quantity, received_at FROM receipt";

    public ReceiptDal(string connectionString) : base(connectionString) { }

    private static Receipt Map(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        ProductId = r.GetInt32(1),
        UserId = r.GetInt32(2),
        Quantity = r.GetInt32(3),
        ReceivedAt = r.GetDateTime(4)
    };
    public int Create(Receipt receipt) =>
        Insert("INSERT INTO receipt (product_id, user_id, quantity) VALUES (@product_id, @user_id, @quantity)",
            P("@product_id", receipt.ProductId),
            P("@user_id", receipt.UserId),
            P("@quantity", receipt.Quantity));

    public Receipt? GetById(int id) =>
        QuerySingle(Select + " WHERE id = @id", Map, P("@id", id));

    public IReadOnlyList<Receipt> GetAll() =>
        Query(Select + " ORDER BY id", Map);

    public bool Update(Receipt receipt) =>
        Execute("UPDATE receipt SET product_id = @product_id, user_id = @user_id, quantity = @quantity WHERE id = @id",
            P("@product_id", receipt.ProductId),
            P("@user_id", receipt.UserId),
            P("@quantity", receipt.Quantity),
            P("@id", receipt.Id)) > 0;

    public bool Delete(int id) =>
        Execute("DELETE FROM receipt WHERE id = @id", P("@id", id)) > 0;
}
