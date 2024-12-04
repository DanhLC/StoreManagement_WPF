using System.Globalization;

namespace StoreManagement.Formatting
{
    public class FormatService : IFormatService
    {
        private readonly CultureInfo _culture;

        public FormatService(CultureInfo culture)
        {
            _culture = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        public string FormatToCurrency(decimal value)
        {
            try
            {
                var parts = value.ToString("F", CultureInfo.InvariantCulture).Split('.');

                var integerPart = parts[0];
                integerPart = string.Format("{0:N0}", decimal.Parse(integerPart))
                                     .Replace(",", ".");

                var fractionalPart = parts.Length > 1 ? "," + parts[1] : string.Empty;

                return integerPart + fractionalPart;
            }
            catch 
            { 
                return string.Empty;
            }
        }

        public decimal ParseCurrencyFormat(string formattedValue)
        {
            try
            {
                var normalized = formattedValue.Replace(".", "").Replace(",", ".");
                return decimal.Parse(normalized, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0m;
            }
        }
    }
}
