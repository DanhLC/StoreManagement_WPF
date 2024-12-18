using CommunityToolkit.Mvvm.ComponentModel;
using StoreManagement.Core;
using StoreManagement.Core.Interfaces.Builders;
using StoreManagement.Core.Interfaces.Services;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace StoreManagement.UI.ViewModels
{
    public class CustomerInputViewModel : ObservableObject
    {
        #region Fields

        private readonly IRepository<Customers> _customerRepository;
        private readonly ICustomerBuilder _customerBuilder;

        #endregion

        #region Properties

        public event Action RequestClose;
        public event Action GoToHomePageRequested;
        public event Action<string, string> ShowMessage;
        public event Func<string, string, bool> ShowMessageConfirm;
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
            ICustomerBuilder customerBuilder)
        {
            _customerRepository = customerRepository;
            _customerBuilder = customerBuilder;

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Customer.FullName))
            {
                ErrorMessage = "FullName is required.";
                return;
            }

            var validEmailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";

            if (!string.IsNullOrWhiteSpace(Customer.Email) && !Regex.IsMatch(Customer.Email , validEmailPattern))
            {
                ErrorMessage = "Invalid email.";
                return;
            }

            var isInsertAction = (Customer.Id == 0);

            try
            {
                var isConfirmed = ShowMessageConfirm.Invoke("Question", string.Format("Do you want to {0}?", isInsertAction ? "insert" : "update"));

                if (!isConfirmed) return;

                if (isInsertAction)
                {
                    var customer = _customerBuilder 
                        .SetCurrentCustomer(Customer)
                        .SetDebtAmount(Customer.DebtAmountString ?? "0")
                        .Build();
                    await _customerRepository.AddAsync(customer);
                }
                else
                {
                    await _customerRepository.UpdateSpecificPropertiesAsync(
                        e => e.Id == Customer.Id,
                        entity => _customerBuilder
                            .SetCurrentCustomer(Customer)
                            .SetDebtAmount(Customer.DebtAmountString ?? "0")
                            .BuildForUpdate(updatedEntity =>
                            {
                                entity.FullName = updatedEntity.FullName;
                                entity.Phone = updatedEntity.Phone;
                                entity.Address = updatedEntity.Address;
                                entity.Email = updatedEntity.Email;
                                entity.DebtAmount = updatedEntity.DebtAmount;
                            }));
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
