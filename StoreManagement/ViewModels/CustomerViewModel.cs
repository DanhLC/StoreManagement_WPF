using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Formatting;
using StoreManagement.Models;
using StoreManagement.Services;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreManagement.ViewModels
{
    public class CustomerViewModel : ObservableObject
    {
        #region Fields

        private readonly IRepository<Customers> _customerRepository;
        private readonly IFormatService _formatService;
        private readonly IViewFactory _viewFactory;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Properties

        public event Func<string, string, bool> ShowMessageConfirm;
        public event Action<string, string> ShowMessage;
        public ObservableCollection<Customers> Customers { get; set; }
        public int TotalResults { get; private set; }
        public int PageSize { get; private set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
        private string _customerName;

        private int _currentPage;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value && (value > 0 && value < (TotalPages + 1)))
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;
                    OnPropertyChanged(nameof(CustomerName));
                }
            }
        }

        private string _phone;

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        private bool _isAllSelected;

        public bool IsAllSelected
        {
            get => _isAllSelected;
            set
            {
                if (_isAllSelected != value)
                {
                    _isAllSelected = value;
                    OnPropertyChanged(nameof(IsAllSelected));

                    foreach (var customer in Customers)
                    {
                        customer.IsSelected = value;
                    }

                    RefreshCustomers();
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand GoToPageCommand { get; }
        public ICommand LoadPageCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand MultipleDeleteCommand { get; }
        public ICommand GoToPreviousPageCommand { get; }
        public ICommand GoToNextPageCommand { get; }
        public ICommand GoToHomePageCommand { get; }
        public ICommand GoToEndPageCommand { get; }
        public ICommand GotoSpecificPageCommand { get; }

        #endregion

        #region Ctor

        public CustomerViewModel(
            IRepository<Customers> customerRepository,
            IFormatService formatService,
            IViewFactory viewFactory,
            IServiceProvider serviceProvider)
        {
            _customerRepository = customerRepository;
            _formatService = formatService;
            _viewFactory = viewFactory;
            _serviceProvider = serviceProvider;
            Customers = new ObservableCollection<Customers>();

            LoadPageCommand = new RelayCommand(async _ => await LoadPageAsync(CurrentPage));
            GoToPageCommand = new RelayCommand<int>(async pageIndex => await LoadPageAsync(pageIndex));
            DeleteCommand = new RelayCommand<Customers>(OnDelete);
            MultipleDeleteCommand = new RelayCommand(async _ => await OnMultipleDelete());
            EditCommand = new RelayCommand<Customers>(OnEdit);
            AddCommand = new RelayCommand(_ => OnAdd());

            GoToNextPageCommand = new RelayCommand(
                async _ => await LoadPageAsync(CurrentPage + 1),
                _ => CurrentPage < TotalPages);
            GoToPreviousPageCommand = new RelayCommand(
                async _ => await LoadPageAsync(CurrentPage - 1),
                _ => CurrentPage > 1);
            GoToHomePageCommand = new RelayCommand(
                async _ => await LoadPageAsync(1),
                _ => CurrentPage > 1);
            GoToEndPageCommand = new RelayCommand(
                async _ => await LoadPageAsync(TotalPages),
                _ => CurrentPage < TotalPages);
            GotoSpecificPageCommand = new RelayCommand(
                async _ => await LoadPageAsync(CurrentPage),
                _ => (CurrentPage != 0 && CurrentPage < (TotalPages + 1)));
            _serviceProvider = serviceProvider;
        }

        #endregion

        public async Task LoadPageAsync(int pageIndex)
        {
            Expression<Func<Customers, bool>> predicate = customer =>
                   (string.IsNullOrWhiteSpace(CustomerName) || customer.FullName.Contains(CustomerName)) &&
                   (string.IsNullOrWhiteSpace(Phone) || customer.Phone.Contains(Phone));
            var (items, totalCount) = await _customerRepository.GetPagedAsync(
                 pageIndex: pageIndex,
                 pageSize: PageSize,
                 predicate: predicate,
                 orderBy: customer => customer.Id,
                 isDescending: true
             );

            Customers.Clear();
            var identityNo = 1;
            var colors = new string[]
            {
                 "#1098AD", "#1E88E5", "#FF8F00", "#FF5252", "#0CA678", "#6741D9", "#FF6D00"
            };
            var random = new Random();

            foreach (var customer in items)
            {
                var randomColor = colors[random.Next(colors.Length)];
                var converter = new BrushConverter();
                customer.IdentityNumber = identityNo;
                customer.BgColor = randomColor;
                customer.Character = string.IsNullOrWhiteSpace(customer.FullName) ? string.Empty
                    : customer.FullName[0].ToString().ToUpper();
                var debtAmount = customer.DebtAmount;
                customer.DebtAmountString = _formatService.FormatToCurrency(debtAmount);
                Customers.Add(customer);
                identityNo++;
            }

            TotalResults = totalCount;
            CurrentPage = pageIndex;

            OnPropertyChanged(nameof(TotalResults));
            OnPropertyChanged(nameof(Customers));
            OnPropertyChanged(nameof(TotalPages));
        }

        public void UpdatePageSize(int rows)
        {
            PageSize = rows;

            OnPropertyChanged(nameof(PageSize));
            OnPropertyChanged(nameof(TotalPages));
        }

        private void RefreshCustomers()
        {
            var tempCustomers = Customers.ToList();
            Customers.Clear();

            foreach (var customer in tempCustomers)
            {
                Customers.Add(customer);
            }

            OnPropertyChanged(nameof(Customers));
        }

        private async void OnDelete(Customers customer)
        {
            if (customer == null) return;

            var isConfirmed = ShowMessageConfirm.Invoke("Delete Confirmation", string.Format("Are you sure you want to delete customer: {0}, Address: {1}?", customer.FullName, customer.Address));

            if (isConfirmed)
            {
                await _customerRepository.DeleteByIdAsync(customer.Id);
                Customers.Remove(customer);
                MessageBox.Show(string.Format("Customer: {0}, Address: {1} has been deleted.", customer.FullName, customer.Address));
                await LoadPageAsync(CurrentPage);
            }
        }

        private void OnEdit(Customers customer)
        {
            if (customer == null) return;

            var customerInputViewModel = _serviceProvider.GetRequiredService<CustomerInputViewModel>();
            customerInputViewModel.Customer = _formatService.DeepCopyUsingJson(customer);
            _viewFactory.OpenViewInput("CustomerInput", customerInputViewModel, 450, 450, "Edit Customer");
        }

        private async Task OnMultipleDelete()
        {
            if (!Customers.Any(e => e.IsSelected)) ShowMessage?.Invoke("Error", "Could you please specify which information you would like to delete?");
            else
            {
                try
                {
                    var isConfirmed = ShowMessageConfirm.Invoke(
                        "Delete Confirmation",
                        string.Format("Are you sure you want to delete customers: {0}?",
                            string.Join(", ",
                                Customers
                                    .Where(e => e.IsSelected)
                                    .Select(e => e.FullName))));

                    if (!isConfirmed) return;

                    var validCusomers = Customers.Where(e => e.IsSelected);
                    await _customerRepository.DeleteManyAsync(item => validCusomers.Select(e => e.Id).ToList().Contains(item.Id));
                    ShowMessage?.Invoke("Success", "The data has been successfully deleted.");
                }
                catch (Exception ex)
                {
                    ShowMessage?.Invoke("Error", string.Format("* {0}", ex.Message));
                }
            }
        }

        private void OnAdd()
        {
            var customerInputViewModel = _serviceProvider.GetRequiredService<CustomerInputViewModel>();
            customerInputViewModel.Customer = new Customers();
            _viewFactory.OpenViewInput("CustomerInput", customerInputViewModel, 450, 450, "Edit Customer");
        }
    }
}
