using System.Collections.ObjectModel;
using System.Windows.Input;
using ReceivingSystem.Bll.Services;
using ReceivingSystem.Dal.Models;
using ReceivingSystem.Wpf.Commands;

namespace ReceivingSystem.Wpf.ViewModels;

public class ReceiverViewModel : ViewModelBase
{
    private readonly IReceiverService _receiverService;
    private readonly User _currentUser = new() { Id = 1, Username = "receiver_user", RoleId = 2 };

    public ObservableCollection<Category> Categories { get; set; } = new();
    public ObservableCollection<Product> Products { get; set; } = new();
    public ObservableCollection<Receipt> Receipts { get; set; } = new();

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set { _selectedCategory = value; OnPropertyChanged(); LoadProducts(); }
    }

    private Product? _selectedProduct;
    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set { _selectedProduct = value; OnPropertyChanged(); }
    }

    private string _quantityInput = string.Empty;
    public string QuantityInput
    {
        get => _quantityInput;
        set { _quantityInput = value; OnPropertyChanged(); }
    }

    private string _searchFilter = string.Empty;
    public string SearchFilter
    {
        get => _searchFilter;
        set { _searchFilter = value; OnPropertyChanged(); LoadReceipts(); }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public ICommand AddReceiptCommand { get; }
    public ICommand RefreshCommand { get; }

    public ReceiverViewModel(IReceiverService receiverService)
    {
        _receiverService = receiverService;
        AddReceiptCommand = new RelayCommand(ExecuteAddReceipt);
        RefreshCommand = new RelayCommand(_ => LoadData());
        LoadData();
    }

    private void LoadData()
    {
        Categories.Clear();
        foreach (var c in _receiverService.GetCategories()) Categories.Add(c);
        LoadReceipts();
    }

    private void LoadProducts()
    {
        Products.Clear();
        if (SelectedCategory != null)
            foreach (var p in _receiverService.GetProductsByCategoryId(SelectedCategory.Id)) Products.Add(p);
    }

    private void LoadReceipts()
    {
        Receipts.Clear();
        foreach (var r in _receiverService.GetReceipts(SearchFilter)) Receipts.Add(r);
    }

    private void ExecuteAddReceipt(object? obj)
    {
        if (SelectedProduct == null) { ErrorMessage = "Оберіть товар."; return; }
        if (!int.TryParse(QuantityInput, out int qty)) { ErrorMessage = "Введіть число."; return; }
        if (!_receiverService.AddReceipt(SelectedProduct.Id, _currentUser.Id, qty, out string err)) { ErrorMessage = err; return; }
        ErrorMessage = string.Empty;
        QuantityInput = string.Empty;
        LoadReceipts();
    }
}
