using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.Dal.Interfaces;

public interface IReceiptDal
{
    int Create(Receipt receipt);

    Receipt? GetById(int id);
    IReadOnlyList<Receipt> GetAll();
    bool Update(Receipt receipt);

    bool Delete(int id);
}
