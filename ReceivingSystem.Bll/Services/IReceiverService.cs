using ReceivingSystem.Dal.Models;
namespace ReceivingSystem.Bll.Services;
public interface IReceiverService
{
    IEnumerable<Category> GetCategories();
    IEnumerable<Product> GetProductsByCategoryId(int categoryId);
    IEnumerable<Receipt> GetReceipts(string searchFilter = "");
    bool AddReceipt(int productId, int userId, int quantity, out string errorMessage);
}
