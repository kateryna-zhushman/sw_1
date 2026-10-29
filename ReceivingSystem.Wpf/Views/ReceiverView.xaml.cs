using System.Windows;
namespace ReceivingSystem.Wpf.Views;
public partial class ReceiverView : Window
{
    public ReceiverView(ViewModels.ReceiverViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
