using CommunityToolkit.Mvvm.ComponentModel;
using StoreManagement.Formatting;
using StoreManagement.Models;
using StoreManagement.Services;
using System.Windows.Input;

namespace StoreManagement.ViewModels
{
    public class CustomerInputViewModel : ObservableObject
    {
        #region Fields

        private readonly IRepository<Customers> _customerRepository;
        private readonly IFormatService _formatService;

        #endregion

        #region Properties

        public event Action RequestClose;
        public event Action GoToHomePageRequested;
        public event Action<string, string> ShowMessage;

        public Customers Customer { get; set;  }
        private string _errorMessage;

        public string ErrorMessage
        {
            get {
                return _errorMessage;
            }
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }

        #endregion

        public CustomerInputViewModel(
            IRepository<Customers> customerRepository,
            IFormatService formatService)
        {
            _customerRepository = customerRepository;
            _formatService = formatService;

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Customer.FullName))
            {
                ErrorMessage = "FullName is required.";
                return;
            }

            var isInsertAction = (Customer.Id == 0);
            var debtAmount = _formatService.ParseCurrencyFormat(Customer.DebtAmountString);

            try
            {
                if (isInsertAction)
                {
                    Customer.DebtAmount = debtAmount;
                    await _customerRepository.AddAsync(Customer);
                }
                else
                {
                    await _customerRepository.UpdateSpecificPropertiesAsync(
                        e => e.Id == Customer.Id,
                        entity =>
                        {
                            entity.FullName = Customer.FullName;
                            entity.Phone = Customer.Phone;
                            entity.Address = Customer.Address;
                            entity.Email = Customer.Email;
                            entity.DebtAmount = debtAmount;
                        });
                }

                ShowMessage?.Invoke("Success", "You have successfully inserted or updated the data.");
                GoToHomePageRequested?.Invoke();
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("* {0}", ex.Message);
            }
        }
    }
}
