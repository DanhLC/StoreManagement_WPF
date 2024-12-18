namespace StoreManagement.Core.Interfaces.Builders
{
    public interface ICustomerBuilder
    {
        ICustomerBuilder SetFullName(string fullName);
        ICustomerBuilder SetPhone(string phone);
        ICustomerBuilder SetAddress(string address);
        ICustomerBuilder SetEmail(string email);
        ICustomerBuilder SetDebtAmount(string debtAmountString);
        ICustomerBuilder SetCurrentCustomer(Customers customer);
        ICustomerBuilder SetDebtAmountString(decimal debtAmount);
        ICustomerBuilder SetIdentityNumber(int identityNo);
        ICustomerBuilder SetBgColor(string[] colors, Random random);
            ICustomerBuilder SetCharacter();
        Customers Build();
        ICustomerBuilder AddToList();
        List<Customers> BuildList();
        void BuildForUpdate(Action<Customers> updateAction);
    }
}
