using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Interfaces;

public interface ICategoryDal
{
    int Create(Category category);
    Category? GetById(int id);
    IReadOnlyList<Category> GetAll();
    bool Update(Category category);
    bool Delete(int id);
}
