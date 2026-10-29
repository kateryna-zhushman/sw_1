using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Interfaces;
public interface ICategoryDal { IEnumerable<Category> GetAll(); }