using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Sql;
public class ReceiptDal : IReceiptDal
{
    private static readonly List<Receipt> _items = new();
    public ReceiptDal(string connectionString) { }
    public void Add(Receipt receipt) { receipt.Id = _items.Count + 1; _items.Add(receipt); }
    public IEnumerable<Receipt> GetAll(string filter = "") => 
        string.IsNullOrWhiteSpace(filter) ? _items : _items.Where(r => r.ProductId.ToString().Contains(filter) || r.UserId.ToString().Contains(filter));
}
