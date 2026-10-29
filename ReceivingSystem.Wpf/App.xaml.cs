using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ReceivingSystem.Bll.Services;
using ReceivingSystem.Dal;
using ReceivingSystem.Dal.Interfaces;
using ReceivingSystem.Dal.Sql;
using ReceivingSystem.Wpf.ViewModels;
using ReceivingSystem.Wpf.Views;

namespace ReceivingSystem.Wpf;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        string connStr = DbSettings.ConnectionString;

        services.AddSingleton<IRoleDal>(s => new RoleDal(connStr));
        services.AddSingleton<IUserDal>(s => new UserDal(connStr));
        services.AddSingleton<ICategoryDal>(s => new CategoryDal(connStr));
        services.AddSingleton<IProductDal>(s => new ProductDal(connStr));
        services.AddSingleton<IReceiptDal>(s => new ReceiptDal(connStr));

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IReceiverService, ReceiverService>();

        services.AddTransient<ReceiverViewModel>();
        services.AddTransient<ReceiverView>();

        ServiceProvider = services.BuildServiceProvider();

        var mainView = ServiceProvider.GetRequiredService<ReceiverView>();
        mainView.Show();
    }
}
