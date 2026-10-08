using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Dal.Sql;

namespace ReceivingSystem.Dal.Tests;

public class ReceiptDalTests : DalTestBase
{
    private readonly IReceiptDal _dal;

    public ReceiptDalTests()
    {
        _dal = new ReceiptDal(ConnectionString);
    }

    private Receipt NewReceipt(int quantity = 10)
    {
        var receipt = new Receipt
        {
            ProductId = NewProduct().Id,
            UserId = NewUser().Id,
            Quantity = quantity
        };
        receipt.Id = _dal.Create(receipt);
        return receipt;
    }

    [Fact]
    public void Create_ReturnsPositiveId_AndReceiptCanBeRead()
    {
        var product = NewProduct();
        var user = NewUser();
        var receipt = new Receipt { ProductId = product.Id, UserId = user.Id, Quantity = 25 };

        var id = _dal.Create(receipt);
        var loaded = _dal.GetById(id);

        Assert.True(id > 0);
        Assert.NotNull(loaded);
        Assert.Equal(product.Id, loaded!.ProductId);
        Assert.Equal(user.Id, loaded.UserId);
        Assert.Equal(25, loaded.Quantity);
    }

    [Fact]
    public void Create_SetsReceivedAtAutomatically()
    {
        var receipt = NewReceipt();

        var loaded = _dal.GetById(receipt.Id);

        Assert.NotNull(loaded);
        Assert.True(Math.Abs((DateTime.Now - loaded!.ReceivedAt).TotalMinutes) < 10,
            $"Дата приймання {loaded.ReceivedAt:O} не схожа на поточний час.");
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        Assert.Null(_dal.GetById(int.MaxValue));
    }

    [Fact]
    public void GetAll_ContainsCreatedReceipt()
    {
        var receipt = NewReceipt();

        var all = _dal.GetAll();

        Assert.Contains(all, r => r.Id == receipt.Id);
    }

    [Fact]
    public void Update_ChangesStoredValues_ButNotDate()
    {
        var receipt = NewReceipt(quantity: 10);
        var dateBefore = _dal.GetById(receipt.Id)!.ReceivedAt;
        var anotherProduct = NewProduct();
        var anotherUser = NewUser();
        receipt.ProductId = anotherProduct.Id;
        receipt.UserId = anotherUser.Id;
        receipt.Quantity = 99;

        var updated = _dal.Update(receipt);
        var loaded = _dal.GetById(receipt.Id);

        Assert.True(updated);
        Assert.Equal(anotherProduct.Id, loaded!.ProductId);
        Assert.Equal(anotherUser.Id, loaded.UserId);
        Assert.Equal(99, loaded.Quantity);
        Assert.Equal(dateBefore, loaded.ReceivedAt);
    }

    [Fact]
    public void Update_UnknownId_ReturnsFalse()
    {
        var ghost = new Receipt
        {
            Id = int.MaxValue,
            ProductId = NewProduct().Id,
            UserId = NewUser().Id,
            Quantity = 1
        };

        Assert.False(_dal.Update(ghost));
    }

    [Fact]
    public void Delete_RemovesReceipt()
    {
        var receipt = NewReceipt();

        var deleted = _dal.Delete(receipt.Id);

        Assert.True(deleted);
        Assert.Null(_dal.GetById(receipt.Id));
    }

    [Fact]
    public void Delete_UnknownId_ReturnsFalse()
    {
        Assert.False(_dal.Delete(int.MaxValue));
    }
}
