using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Interfaces;
public interface IProductDal { IEnumerable<Product> GetByCategoryId(int categoryId); }