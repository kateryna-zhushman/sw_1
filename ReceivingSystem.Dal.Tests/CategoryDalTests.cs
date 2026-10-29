using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Dal.Sql;

namespace ReceivingSystem.Dal.Tests;

public class CategoryDalTests : DalTestBase
{
    private readonly ICategoryDal _dal;

    public CategoryDalTests()
    {
        _dal = new CategoryDal(ConnectionString);
    }

    [Fact]
    public void Create_ReturnsPositiveId_AndCategoryCanBeRead()
    {
        var category = new Category { Name = Unique("category") };

        var id = _dal.Create(category);
        var loaded = _dal.GetById(id);

        Assert.True(id > 0);
        Assert.NotNull(loaded);
        Assert.Equal(category.Name, loaded!.Name);
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        Assert.Null(_dal.GetById(int.MaxValue));
    }

    [Fact]
    public void GetAll_ContainsCreatedCategory()
    {
        var category = NewCategory();

        var all = _dal.GetAll();

        Assert.Contains(all, c => c.Id == category.Id && c.Name == category.Name);
    }

    [Fact]
    public void Update_ChangesStoredName()
    {
        var category = NewCategory();
        category.Name = Unique("renamed");

        var updated = _dal.Update(category);
        var loaded = _dal.GetById(category.Id);

        Assert.True(updated);
        Assert.Equal(category.Name, loaded!.Name);
    }

    [Fact]
    public void Update_UnknownId_ReturnsFalse()
    {
        Assert.False(_dal.Update(new Category { Id = int.MaxValue, Name = "ghost" }));
    }

    [Fact]
    public void Delete_RemovesCategory()
    {
        var category = NewCategory();

        var deleted = _dal.Delete(category.Id);

        Assert.True(deleted);
        Assert.Null(_dal.GetById(category.Id));
    }

    [Fact]
    public void Delete_UnknownId_ReturnsFalse()
    {
        Assert.False(_dal.Delete(int.MaxValue));
    }

    [Fact]
    public void Delete_CategoryUsedByProduct_ThrowsBecauseOfForeignKey()
    {
        var product = NewProduct();

        Assert.Throws<SqlException>(() => _dal.Delete(product.CategoryId));
    }
}
