using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Interfaces;

public interface IProductDal
{
    int Create(Product product);
    Product? GetById(int id);
    IReadOnlyList<Product> GetAll();
    bool Update(Product product);
    bool Delete(int id);
    IReadOnlyList<Product> GetByCategory(int categoryId);
}
