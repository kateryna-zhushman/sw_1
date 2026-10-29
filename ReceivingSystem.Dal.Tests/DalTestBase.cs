using System.Transactions;
using ReceivingSystem.Dal;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Dal.Sql;

namespace ReceivingSystem.Dal.Tests;

public abstract class DalTestBase : IDisposable
{
    private readonly TransactionScope _scope;

    protected string ConnectionString { get; } = DbSettings.ConnectionString;

    protected DalTestBase()
    {
        _scope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(2)
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    protected static string Unique(string prefix) =>
        prefix + "_" + Guid.NewGuid().ToString("N")[..8];


    protected Role NewRole()
    {
        var role = new Role { Name = Unique("role"), PageUrl = "/test" };
        role.Id = new RoleDal(ConnectionString).Create(role);
        return role;
    }

    protected Category NewCategory()
    {
        var category = new Category { Name = Unique("category") };
        category.Id = new CategoryDal(ConnectionString).Create(category);
        return category;
    }

    protected User NewUser()
    {
        var role = NewRole();
        var user = new User { Username = Unique("user"), PasswordHash = "hash_test", RoleId = role.Id };
        user.Id = new UserDal(ConnectionString).Create(user);
        return user;
    }

    protected Product NewProduct()
    {
        var category = NewCategory();
        var product = new Product { Name = Unique("product"), CategoryId = category.Id };
        product.Id = new ProductDal(ConnectionString).Create(product);
        return product;
    }
}
