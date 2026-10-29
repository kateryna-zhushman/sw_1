using Microsoft.Data.SqlClient;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Dal.Sql;

namespace ReceivingSystem.Dal.Tests;

public class RoleDalTests : DalTestBase
{
    private readonly IRoleDal _dal;

    public RoleDalTests()
    {
        _dal = new RoleDal(ConnectionString);
    }

    [Fact]
    public void Create_ReturnsPositiveId_AndRoleCanBeRead()
    {
        var role = new Role { Name = Unique("role"), PageUrl = "/receiver" };

        var id = _dal.Create(role);
        var loaded = _dal.GetById(id);

        Assert.True(id > 0);
        Assert.NotNull(loaded);
        Assert.Equal(id, loaded!.Id);
        Assert.Equal(role.Name, loaded.Name);
        Assert.Equal("/receiver", loaded.PageUrl);
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        Assert.Null(_dal.GetById(int.MaxValue));
    }

    [Fact]
    public void GetAll_ContainsCreatedRole()
    {
        var role = NewRole();

        var all = _dal.GetAll();

        Assert.Contains(all, r => r.Id == role.Id && r.Name == role.Name);
    }

    [Fact]
    public void Update_ChangesStoredValues()
    {
        var role = NewRole();
        role.Name = Unique("renamed");
        role.PageUrl = "/changed";

        var updated = _dal.Update(role);
        var loaded = _dal.GetById(role.Id);

        Assert.True(updated);
        Assert.Equal(role.Name, loaded!.Name);
        Assert.Equal("/changed", loaded.PageUrl);
    }

    [Fact]
    public void Update_UnknownId_ReturnsFalse()
    {
        var ghost = new Role { Id = int.MaxValue, Name = "ghost", PageUrl = "/ghost" };

        Assert.False(_dal.Update(ghost));
    }

    [Fact]
    public void Delete_RemovesRole()
    {
        var role = NewRole();

        var deleted = _dal.Delete(role.Id);

        Assert.True(deleted);
        Assert.Null(_dal.GetById(role.Id));
    }

    [Fact]
    public void Delete_UnknownId_ReturnsFalse()
    {
        Assert.False(_dal.Delete(int.MaxValue));
    }

    [Fact]
    public void Delete_RoleUsedByUser_ThrowsBecauseOfForeignKey()
    { 
        var user = NewUser();

        Assert.Throws<SqlException>(() => _dal.Delete(user.RoleId));
    }
}
