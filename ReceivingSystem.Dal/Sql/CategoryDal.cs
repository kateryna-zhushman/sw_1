using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Sql;
public class CategoryDal : ICategoryDal
{
    public CategoryDal(string connectionString) { }
    public IEnumerable<Category> GetAll() => new List<Category> {
        new Category { Id = 1, Name = "Продукти харчування" },
        new Category { Id = 2, Name = "Побутова хімія" }
    };
}
