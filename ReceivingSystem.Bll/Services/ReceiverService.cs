using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Bll.Services;
public class ReceiverService : IReceiverService
{
    private readonly ICategoryDal _categoryDal;
    private readonly IProductDal _productDal;
    private readonly IReceiptDal _receiptDal;
    public ReceiverService(ICategoryDal categoryDal, IProductDal productDal, IReceiptDal receiptDal)
    {
        _categoryDal = categoryDal;
        _productDal = productDal;
        _receiptDal = receiptDal;
    }
    public IEnumerable<Category> GetCategories() => _categoryDal.GetAll();
    public IEnumerable<Product> GetProductsByCategoryId(int catId) => _productDal.GetByCategoryId(catId);
    public IEnumerable<Receipt> GetReceipts(string filter = "") => _receiptDal.GetAll(filter);
    public bool AddReceipt(int productId, int userId, int quantity, out string error)
    {
        if (quantity <= 0) { error = "Кількість повинна бути більшою за 0."; return false; }
        if (productId <= 0) { error = "Необхідно обрати товар."; return false; }
        _receiptDal.Add(new Receipt { ProductId = productId, UserId = userId, Quantity = quantity, ReceivedAt = DateTime.Now });
        error = string.Empty;
        return true;
    }
}
