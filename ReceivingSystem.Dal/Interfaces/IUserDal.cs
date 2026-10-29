using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Interfaces;

public interface IUserDal
{
    int Create(User user);
    User? GetById(int id);
    IReadOnlyList<User> GetAll();
    bool Update(User user);
    bool Delete(int id);
    User? GetByUsername(string username);
}
