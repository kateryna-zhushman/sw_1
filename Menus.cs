using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Models;

namespace ReceivingSystem.ConsoleApp;

internal sealed class Menus
{
    private readonly IRoleDal _roles;
    private readonly ICategoryDal _categories;
    private readonly IUserDal _users;
    private readonly IProductDal _products;
    private readonly IReceiptDal _receipts;

    public Menus(IRoleDal roles, ICategoryDal categories, IUserDal users, IProductDal products, IReceiptDal receipts)
    {
        _roles = roles;
        _categories = categories;
        _users = users;
        _products = products;
        _receipts = receipts;
    }
    public void Roles() => CrudMenu.Run("Ролі (roles)",
        list: () =>
        {
            var items = _roles.GetAll();
            Console.WriteLine($"{"Id",4} | {"Name",-20} | PageUrl");
            foreach (var r in items) Console.WriteLine(Format(r));
            Console.WriteLine($"Всього: {items.Count}");
        },
        find: () =>
        {
            var role = _roles.GetById(ConsoleIo.ReadInt("Id ролі"));
            Console.WriteLine(role is null ? "Не знайдено." : Format(role));
        },
        create: () =>
        {
            var role = new Role
            {
                Name = ConsoleIo.ReadString("Назва ролі"),
                PageUrl = ConsoleIo.ReadString("Адреса сторінки (наприклад /receiver)")
            };
            role.Id = _roles.Create(role);
            ConsoleIo.Info($"Додано. Новий Id = {role.Id}");
        },
        update: () =>
        {
            var role = _roles.GetById(ConsoleIo.ReadInt("Id ролі для зміни"));
            if (role is null) { ConsoleIo.Warn("Не знайдено."); return; }

            role.Name = ConsoleIo.ReadString("Назва", role.Name);
            role.PageUrl = ConsoleIo.ReadString("Адреса сторінки", role.PageUrl);
            ConsoleIo.Info(_roles.Update(role) ? "Оновлено." : "Нічого не оновлено.");
        },
        delete: () =>
        {
            var id = ConsoleIo.ReadInt("Id ролі для видалення");
            ConsoleIo.Info(_roles.Delete(id) ? "Видалено." : "Запису з таким Id немає.");
        });

    private static string Format(Role r) => $"{r.Id,4} | {r.Name,-20} | {r.PageUrl}";

    public void Categories() => CrudMenu.Run("Категорії (categories)",
        list: () =>
        {
            var items = _categories.GetAll();
            Console.WriteLine($"{"Id",4} | Name");
            foreach (var c in items) Console.WriteLine(Format(c));
            Console.WriteLine($"Всього: {items.Count}");
        },
        find: () =>
        {
            var category = _categories.GetById(ConsoleIo.ReadInt("Id категорії"));
            Console.WriteLine(category is null ? "Не знайдено." : Format(category));
        },
        create: () =>
        {
            var category = new Category { Name = ConsoleIo.ReadString("Назва категорії") };
            category.Id = _categories.Create(category);
            ConsoleIo.Info($"Додано. Новий Id = {category.Id}");
        },
        update: () =>
        {
            var category = _categories.GetById(ConsoleIo.ReadInt("Id категорії для зміни"));
            if (category is null) { ConsoleIo.Warn("Не знайдено."); return; }

            category.Name = ConsoleIo.ReadString("Назва", category.Name);
            ConsoleIo.Info(_categories.Update(category) ? "Оновлено." : "Нічого не оновлено.");
        },
        delete: () =>
        {
            var id = ConsoleIo.ReadInt("Id категорії для видалення");
            ConsoleIo.Info(_categories.Delete(id) ? "Видалено." : "Запису з таким Id немає.");
        });

    private static string Format(Category c) => $"{c.Id,4} | {c.Name}";

   
    public void Users() => CrudMenu.Run("Користувачі (users)",
        list: () =>
        {
            var items = _users.GetAll();
            Console.WriteLine($"{"Id",4} | {"Username",-15} | {"PasswordHash",-20} | RoleId");
            foreach (var u in items) Console.WriteLine(Format(u));
            Console.WriteLine($"Всього: {items.Count}");
        },
        find: () =>
        {
            Console.WriteLine("Шукати за: 1 - Id, 2 - логіном");
            var byName = ConsoleIo.ReadString("Вибір") == "2";
            var user = byName
                ? _users.GetByUsername(ConsoleIo.ReadString("Логін"))
                : _users.GetById(ConsoleIo.ReadInt("Id користувача"));
            Console.WriteLine(user is null ? "Не знайдено." : Format(user));
        },
        create: () =>
        {
            var user = new User
            {
                Username = ConsoleIo.ReadString("Логін"),
                PasswordHash = ConsoleIo.ReadString("Хеш пароля (умовний)"),
                RoleId = ConsoleIo.ReadPositiveInt("Id ролі")
            };
            user.Id = _users.Create(user);
            ConsoleIo.Info($"Додано. Новий Id = {user.Id}");
        },
        update: () =>
        {
            var user = _users.GetById(ConsoleIo.ReadInt("Id користувача для зміни"));
            if (user is null) { ConsoleIo.Warn("Не знайдено."); return; }

            user.Username = ConsoleIo.ReadString("Логін", user.Username);
            user.PasswordHash = ConsoleIo.ReadString("Хеш пароля", user.PasswordHash);
            user.RoleId = ConsoleIo.ReadPositiveInt("Id ролі", user.RoleId);
            ConsoleIo.Info(_users.Update(user) ? "Оновлено." : "Нічого не оновлено.");
        },
        delete: () =>
        {
            var id = ConsoleIo.ReadInt("Id користувача для видалення");
            ConsoleIo.Info(_users.Delete(id) ? "Видалено." : "Запису з таким Id немає.");
        });

    private static string Format(User u) => $"{u.Id,4} | {u.Username,-15} | {u.PasswordHash,-20} | {u.RoleId}";

    public void Products() => CrudMenu.Run("Товари (products)",
        list: () =>
        {
            Console.WriteLine("Показати: 1 - всі, 2 - лише певної категорії");
            var byCategory = ConsoleIo.ReadString("Вибір") == "2";
            var items = byCategory
                ? _products.GetByCategory(ConsoleIo.ReadInt("Id категорії"))
                : _products.GetAll();

            Console.WriteLine($"{"Id",4} | {"Name",-35} | CategoryId");
            foreach (var p in items) Console.WriteLine(Format(p));
            Console.WriteLine($"Всього: {items.Count}");
        },
        find: () =>
        {
            var product = _products.GetById(ConsoleIo.ReadInt("Id товару"));
            Console.WriteLine(product is null ? "Не знайдено." : Format(product));
        },
        create: () =>
        {
            var product = new Product
            {
                Name = ConsoleIo.ReadString("Назва товару"),
                CategoryId = ConsoleIo.ReadPositiveInt("Id категорії")
            };
            product.Id = _products.Create(product);
            ConsoleIo.Info($"Додано. Новий Id = {product.Id}");
        },
        update: () =>
        {
            var product = _products.GetById(ConsoleIo.ReadInt("Id товару для зміни"));
            if (product is null) { ConsoleIo.Warn("Не знайдено."); return; }

            product.Name = ConsoleIo.ReadString("Назва", product.Name);
            product.CategoryId = ConsoleIo.ReadPositiveInt("Id категорії", product.CategoryId);
            ConsoleIo.Info(_products.Update(product) ? "Оновлено." : "Нічого не оновлено.");
        },
        delete: () =>
        {
            var id = ConsoleIo.ReadInt("Id товару для видалення");
            ConsoleIo.Info(_products.Delete(id) ? "Видалено." : "Запису з таким Id немає.");
        });

    private static string Format(Product p) => $"{p.Id,4} | {p.Name,-35} | {p.CategoryId}";

    // ---------------------------------------------------------------- Receipts

    public void Receipts() => CrudMenu.Run("Приймання товару (receipt)",
        list: () =>
        {
            var items = _receipts.GetAll();
            Console.WriteLine($"{"Id",4} | {"ProductId",9} | {"UserId",6} | {"Qty",5} | ReceivedAt");
            foreach (var r in items) Console.WriteLine(Format(r));
            Console.WriteLine($"Всього: {items.Count}");
        },
        find: () =>
        {
            var receipt = _receipts.GetById(ConsoleIo.ReadInt("Id запису"));
            Console.WriteLine(receipt is null ? "Не знайдено." : Format(receipt));
        },
        create: () =>
        {
            var receipt = new Receipt
            {
                ProductId = ConsoleIo.ReadPositiveInt("Id товару"),
                UserId = ConsoleIo.ReadPositiveInt("Id користувача (приймальника)"),
                Quantity = ConsoleIo.ReadPositiveInt("Кількість")
            };
            receipt.Id = _receipts.Create(receipt);

            var saved = _receipts.GetById(receipt.Id);
            ConsoleIo.Info($"Додано. Новий Id = {receipt.Id}, дата проставлена автоматично: {saved?.ReceivedAt:yyyy-MM-dd HH:mm:ss}");
        },
        update: () =>
        {
            var receipt = _receipts.GetById(ConsoleIo.ReadInt("Id запису для зміни"));
            if (receipt is null) { ConsoleIo.Warn("Не знайдено."); return; }

            receipt.ProductId = ConsoleIo.ReadPositiveInt("Id товару", receipt.ProductId);
            receipt.UserId = ConsoleIo.ReadPositiveInt("Id користувача", receipt.UserId);
            receipt.Quantity = ConsoleIo.ReadPositiveInt("Кількість", receipt.Quantity);
            ConsoleIo.Info(_receipts.Update(receipt) ? "Оновлено (дата приймання не змінюється)." : "Нічого не оновлено.");
        },
        delete: () =>
        {
            var id = ConsoleIo.ReadInt("Id запису для видалення");
            ConsoleIo.Info(_receipts.Delete(id) ? "Видалено." : "Запису з таким Id немає.");
        });

    private static string Format(Receipt r) =>
        $"{r.Id,4} | {r.ProductId,9} | {r.UserId,6} | {r.Quantity,5} | {r.ReceivedAt:yyyy-MM-dd HH:mm:ss}";
}
