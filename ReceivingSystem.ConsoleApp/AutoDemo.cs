using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.ConsoleApp;

internal sealed class AutoDemo
{
    private readonly IRoleDal _roles;
    private readonly ICategoryDal _categories;
    private readonly IUserDal _users;
    private readonly IProductDal _products;
    private readonly IReceiptDal _receipts;

    public AutoDemo(IRoleDal roles, ICategoryDal categories, IUserDal users, IProductDal products, IReceiptDal receipts)
    {
        _roles = roles;
        _categories = categories;
        _users = users;
        _products = products;
        _receipts = receipts;
    }

    public void Run()
    {
        var tag = DateTime.Now.ToString("HHmmss");

        ConsoleIo.Header("CREATE: додаємо записи в усі таблиці");

        var role = new Role { Name = $"demo_role_{tag}", PageUrl = "/demo" };
        role.Id = _roles.Create(role);
        Console.WriteLine($"roles      -> Id = {role.Id}");

        var category = new Category { Name = $"demo_category_{tag}" };
        category.Id = _categories.Create(category);
        Console.WriteLine($"categories -> Id = {category.Id}");

        var user = new User { Username = $"demo_user_{tag}", PasswordHash = "demo_hash", RoleId = role.Id };
        user.Id = _users.Create(user);
        Console.WriteLine($"users      -> Id = {user.Id}");

        var product = new Product { Name = $"Демо-товар {tag}", CategoryId = category.Id };
        product.Id = _products.Create(product);
        Console.WriteLine($"products   -> Id = {product.Id}");

        var receipt = new Receipt { ProductId = product.Id, UserId = user.Id, Quantity = 5 };
        receipt.Id = _receipts.Create(receipt);
        Console.WriteLine($"receipt    -> Id = {receipt.Id}");

        // ------------------------------------------------------------ READ
        ConsoleIo.Header("READ: читаємо створені записи");

        var roleRead = _roles.GetById(role.Id)!;
        var userRead = _users.GetByUsername(user.Username)!;
        var productRead = _products.GetById(product.Id)!;
        var receiptRead = _receipts.GetById(receipt.Id)!;

        Console.WriteLine($"Роль: {roleRead.Name}, сторінка {roleRead.PageUrl}");
        Console.WriteLine($"Користувач (знайдено за логіном): Id={userRead.Id}, RoleId={userRead.RoleId}");
        Console.WriteLine($"Товар: {productRead.Name}, CategoryId={productRead.CategoryId}");
        Console.WriteLine($"Приймання: Quantity={receiptRead.Quantity}, дата (авто) = {receiptRead.ReceivedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Товарів у категорії {category.Id}: {_products.GetByCategory(category.Id).Count}");
        Console.WriteLine($"Усього записів у roles={_roles.GetAll().Count}, categories={_categories.GetAll().Count}, " +
                          $"users={_users.GetAll().Count}, products={_products.GetAll().Count}, receipt={_receipts.GetAll().Count}");

        ConsoleIo.Header("UPDATE: змінюємо записи");

        category.Name += "_updated";
        product.Name += " (оновлено)";
        receipt.Quantity = 50;
        _categories.Update(category);
        _products.Update(product);
        _receipts.Update(receipt);

        Console.WriteLine($"Категорія: {_categories.GetById(category.Id)!.Name}");
        Console.WriteLine($"Товар:     {_products.GetById(product.Id)!.Name}");
        Console.WriteLine($"Кількість: {_receipts.GetById(receipt.Id)!.Quantity} (було 5)");

        ConsoleIo.Header("DELETE: видаляємо створене (у зворотному порядку залежностей)");

        Console.WriteLine($"receipt    видалено: {_receipts.Delete(receipt.Id)}");
        Console.WriteLine($"products   видалено: {_products.Delete(product.Id)}");
        Console.WriteLine($"users      видалено: {_users.Delete(user.Id)}");
        Console.WriteLine($"categories видалено: {_categories.Delete(category.Id)}");
        Console.WriteLine($"roles      видалено: {_roles.Delete(role.Id)}");

        var allGone = _receipts.GetById(receipt.Id) is null
                      && _products.GetById(product.Id) is null
                      && _users.GetById(user.Id) is null
                      && _categories.GetById(category.Id) is null
                      && _roles.GetById(role.Id) is null;

        if (allGone)
            ConsoleIo.Info("Перевірка: усі демонстраційні записи видалено, база в початковому стані.");
        else
            ConsoleIo.Warn("Увага: частину демонстраційних записів видалити не вдалося.");
    }
}
