using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Sql;
public class RoleDal : IRoleDal
{
    public RoleDal(string conn) { }
    public Role? GetById(int id) => new Role { Id = id, Name = id == 2 ? "Receiver" : "User" };
}
