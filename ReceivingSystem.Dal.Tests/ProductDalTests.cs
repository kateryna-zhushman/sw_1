using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Dal.Sql;

namespace ReceivingSystem.Dal.Tests;

public class ProductDalTests : DalTestBase
{
    private readonly IProductDal _dal;

    public ProductDalTests()
    {
        _dal = new ProductDal(ConnectionString);
    }

    [Fact]
    public void Create_ReturnsPositiveId_AndProductCanBeRead()
    {
        var category = NewCategory();
        var product = new Product { Name = Unique("product"), CategoryId = category.Id };

        var id = _dal.Create(product);
        var loaded = _dal.GetById(id);

        Assert.True(id > 0);
        Assert.NotNull(loaded);
        Assert.Equal(product.Name, loaded!.Name);
        Assert.Equal(category.Id, loaded.CategoryId);
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        Assert.Null(_dal.GetById(int.MaxValue));
    }

    [Fact]
    public void GetAll_ContainsCreatedProduct()
    {
        var product = NewProduct();

        var all = _dal.GetAll();

        Assert.Contains(all, p => p.Id == product.Id && p.Name == product.Name);
    }

    [Fact]
    public void GetByCategory_ReturnsOnlyProductsOfThatCategory()
    {
        var category = NewCategory();
        var otherCategory = NewCategory();
        var inCategory = _dal.Create(new Product { Name = Unique("in"), CategoryId = category.Id });
        var outside = _dal.Create(new Product { Name = Unique("out"), CategoryId = otherCategory.Id });

        var result = _dal.GetByCategory(category.Id);

        Assert.Contains(result, p => p.Id == inCategory);
        Assert.DoesNotContain(result, p => p.Id == outside);
        Assert.All(result, p => Assert.Equal(category.Id, p.CategoryId));
    }

    [Fact]
    public void Update_ChangesStoredValues()
    {
        var product = NewProduct();
        var anotherCategory = NewCategory();
        product.Name = Unique("renamed");
        product.CategoryId = anotherCategory.Id;

        var updated = _dal.Update(product);
        var loaded = _dal.GetById(product.Id);

        Assert.True(updated);
        Assert.Equal(product.Name, loaded!.Name);
        Assert.Equal(anotherCategory.Id, loaded.CategoryId);
    }

    [Fact]
    public void Update_UnknownId_ReturnsFalse()
    {
        var category = NewCategory();

        Assert.False(_dal.Update(new Product { Id = int.MaxValue, Name = "ghost", CategoryId = category.Id }));
    }

    [Fact]
    public void Delete_RemovesProduct()
    {
        var product = NewProduct();

        var deleted = _dal.Delete(product.Id);

        Assert.True(deleted);
        Assert.Null(_dal.GetById(product.Id));
    }

    [Fact]
    public void Delete_UnknownId_ReturnsFalse()
    {
        Assert.False(_dal.Delete(int.MaxValue));
    }
}
