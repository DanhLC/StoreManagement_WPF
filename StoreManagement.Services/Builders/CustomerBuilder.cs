using StoreManagement.Core;
using StoreManagement.Core.Interfaces.Builders;
using StoreManagement.Core.Interfaces.Formatting;

namespace StoreManagement.Services.Builders
{
    public class CustomerBuilder : ICustomerBuilder
    {
        private readonly IFormatService _formatService;
        private Customers _customer;
        private readonly List<Customers> _customerList = new();

        public CustomerBuilder(
            IFormatService formatService)
        {
            _formatService = formatService ?? throw new ArgumentNullException(nameof(formatService));
            _customer = new Customers();
        }

        public ICustomerBuilder SetCurrentCustomer(Customers customer)
        {
            _customer = _formatService.DeepCopyUsingJson(customer);
            return this;
        }

        public ICustomerBuilder SetFullName(string fullName)
        {
            _customer.FullName = fullName;
            return this;
        }

        public ICustomerBuilder SetPhone(string phone)
        {
            _customer.Phone = phone;
            return this;
        }

        public ICustomerBuilder SetAddress(string address)
        {
            _customer.Address = address;
            return this;
        }

        public ICustomerBuilder SetEmail(string email)
        {
            _customer.Email = email;
            return this;
        }

        public ICustomerBuilder SetDebtAmount(string debtAmountString)
        {
            _customer.DebtAmount = _formatService.ParseCurrencyFormat(debtAmountString ?? "0");
            return this;
        }

        public ICustomerBuilder SetDebtAmountString(decimal debtAmount)
        {
            _customer.DebtAmountString = _formatService.FormatToCurrency(debtAmount);
            return this;
        }

        public ICustomerBuilder SetIdentityNumber(int identityNo)
        {
            _customer.IdentityNumber = identityNo;
            return this;
        }

        public ICustomerBuilder SetBgColor(string[] colors, Random random)
        {
            _customer.BgColor = colors[random.Next(colors.Length)];
            return this;
        }

        public ICustomerBuilder SetCharacter()
        {
            _customer.Character = string.IsNullOrWhiteSpace(_customer.FullName) ? string.Empty
                : _customer.FullName[0].ToString().ToUpper();
            return this;
        }

        public ICustomerBuilder AddToList()
        {
            _customerList.Add(_customer);
            _customer = new Customers();

            return this;
        }
        public Customers Build()
        {
            return _customer;
        }

        public List<Customers> BuildList()
        {
            return _customerList;
        }

        public void BuildForUpdate(Action<Customers> updateAction)
        {
            updateAction(_customer);
        }
    }
}
