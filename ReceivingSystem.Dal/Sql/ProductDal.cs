using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Sql;
public class ProductDal : IProductDal
{
    public ProductDal(string connectionString) { }
    public IEnumerable<Product> GetByCategoryId(int categoryId) => new List<Product> {
        new Product { Id = 101, Name = "Хліб житній", CategoryId = 1 },
        new Product { Id = 102, Name = "Молоко 2.5%", CategoryId = 1 },
        new Product { Id = 201, Name = "Мило рідке", CategoryId = 2 }
    }.Where(p => p.CategoryId == categoryId);
}
