using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        #endregion

        #region Properties

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
        public ICommand GoToPreviousPageCommand { get; }
        public ICommand GoToNextPageCommand { get; }
        public ICommand GoToHomePageCommand { get; }
        public ICommand GoToEndPageCommand { get; }
        public ICommand GotoSpecificPageCommand { get; }

        #endregion

        public CustomerViewModel(
            IRepository<Customers> customerRepository,
            IFormatService formatService)
        {
            _customerRepository = customerRepository;
            _formatService = formatService;
            Customers = new ObservableCollection<Customers>();

            LoadPageCommand = new RelayCommand(async _ => await LoadPageAsync(CurrentPage));
            GoToPageCommand = new RelayCommand<int>(async pageIndex => await LoadPageAsync(pageIndex));
            DeleteCommand = new RelayCommand<Customers>(OnDelete);
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
        }

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
                var bgColor = (Brush)converter.ConvertFromString(randomColor);

                customer.IdentityNumber = identityNo;
                customer.BgColor = bgColor;
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

            var result = MessageBox.Show(
                string.Format("Are you sure you want to delete {0}, Address {1}?", customer.FullName, customer.Address),
                    "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _customerRepository.DeleteAsync(customer.Id);
                Customers.Remove(customer);
                MessageBox.Show(string.Format("Customer {0}, Address {1} has been deleted.", customer.FullName, customer.Address));
                await LoadPageAsync(CurrentPage);
            }
        }

        private void OnEdit(Customers customer)
        {
            //if (customer == null) return; 

            //var editCustomerView = new DashboardView test2
            //{ test2
            //    DataContext = new DashboardViewModel(_customerRepository, customer.Id)
            //};

            //var window = new Window
            //{
            //    Content = editCustomerView,
            //    Title = "Edit Customer",
            //    Height = 300,
            //    Width = 400
            //};

            //window.ShowDialog();
        }
    }
}
