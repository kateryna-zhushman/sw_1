using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Dal.Interfaces;
public interface IReceiptDal { void Add(Receipt receipt); IEnumerable<Receipt> GetAll(string searchFilter = ""); }