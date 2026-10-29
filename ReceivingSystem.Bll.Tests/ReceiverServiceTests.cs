using Moq;
using ReceivingSystem.Bll.Services;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
using Xunit;

namespace ReceivingSystem.Bll.Tests;

public class ReceiverServiceTests
{
    [Fact]
    public void AddReceipt_ZeroQuantity_ReturnsFalse()
    {
        var mockCat = new Mock<ICategoryDal>();
        var mockProd = new Mock<IProductDal>();
        var mockRec = new Mock<IReceiptDal>();

        var service = new ReceiverService(mockCat.Object, mockProd.Object, mockRec.Object);

        bool result = service.AddReceipt(1, 1, 0, out string error);

        Assert.False(result);
        Assert.Equal("Кількість повинна бути більшою за 0.", error);
    }
}
