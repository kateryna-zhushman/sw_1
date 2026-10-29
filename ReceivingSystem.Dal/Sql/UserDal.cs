using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Sql;
public class UserDal : IUserDal
{
    public UserDal(string conn) { }
    public User? GetByUsername(string u) => new User { Id = 1, Username = u, RoleId = 2 };
}
