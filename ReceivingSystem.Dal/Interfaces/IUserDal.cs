using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Interfaces;
public interface IUserDal { User? GetByUsername(string username); }