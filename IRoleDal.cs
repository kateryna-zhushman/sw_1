using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Interfaces;

public interface IRoleDal
{
    int Create(Role role);
    Role? GetById(int id);

    IReadOnlyList<Role> GetAll();
    bool Update(Role role);
    bool Delete(int id);
}
