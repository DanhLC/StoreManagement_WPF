namespace StoreManagement.Formatting
{
    public interface IFormatService
    {
        string FormatToCurrency(decimal value);
        decimal ParseCurrencyFormat(string value);
        T DeepCopyUsingJson<T>(T source);
    }
}