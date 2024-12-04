using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoreManagement.Formatting;
using StoreManagement.Models;
using StoreManagement.Services;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
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
        public int CurrentPage { get; private set; } = 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
        private string _customerName;

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

        #endregion

        #region Commands

        public ICommand GoToPageCommand { get; }
        public ICommand LoadPageCommand { get; }

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
    }
}
