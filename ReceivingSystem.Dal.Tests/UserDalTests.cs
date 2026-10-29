using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Dal.Sql;

namespace ReceivingSystem.Dal.Tests;

public class UserDalTests : DalTestBase
{
    private readonly IUserDal _dal;

    public UserDalTests()
    {
        _dal = new UserDal(ConnectionString);
    }

    [Fact]
    public void Create_ReturnsPositiveId_AndUserCanBeRead()
    {
        var role = NewRole();
        var user = new User { Username = Unique("user"), PasswordHash = "hash123", RoleId = role.Id };

        var id = _dal.Create(user);
        var loaded = _dal.GetById(id);

        Assert.True(id > 0);
        Assert.NotNull(loaded);
        Assert.Equal(user.Username, loaded!.Username);
        Assert.Equal("hash123", loaded.PasswordHash);
        Assert.Equal(role.Id, loaded.RoleId);
    }

    [Fact]
    public void GetById_UnknownId_ReturnsNull()
    {
        Assert.Null(_dal.GetById(int.MaxValue));
    }

    [Fact]
    public void GetAll_ContainsCreatedUser()
    {
        var user = NewUser();

        var all = _dal.GetAll();

        Assert.Contains(all, u => u.Id == user.Id && u.Username == user.Username);
    }

    [Fact]
    public void GetByUsername_ReturnsMatchingUser()
    {
        var user = NewUser();

        var found = _dal.GetByUsername(user.Username);

        Assert.NotNull(found);
        Assert.Equal(user.Id, found!.Id);
    }

    [Fact]
    public void GetByUsername_UnknownName_ReturnsNull()
    {
        Assert.Null(_dal.GetByUsername(Unique("nobody")));
    }

    [Fact]
    public void Update_ChangesStoredValues()
    {
        var user = NewUser();
        var anotherRole = NewRole();
        user.Username = Unique("renamed");
        user.PasswordHash = "new_hash";
        user.RoleId = anotherRole.Id;

        var updated = _dal.Update(user);
        var loaded = _dal.GetById(user.Id);

        Assert.True(updated);
        Assert.Equal(user.Username, loaded!.Username);
        Assert.Equal("new_hash", loaded.PasswordHash);
        Assert.Equal(anotherRole.Id, loaded.RoleId);
    }

    [Fact]
    public void Update_UnknownId_ReturnsFalse()
    {
        var role = NewRole();
        var ghost = new User { Id = int.MaxValue, Username = "ghost", PasswordHash = "x", RoleId = role.Id };

        Assert.False(_dal.Update(ghost));
    }

    [Fact]
    public void Delete_RemovesUser()
    {
        var user = NewUser();

        var deleted = _dal.Delete(user.Id);

        Assert.True(deleted);
        Assert.Null(_dal.GetById(user.Id));
    }

    [Fact]
    public void Delete_UnknownId_ReturnsFalse()
    {
        Assert.False(_dal.Delete(int.MaxValue));
    }
}
