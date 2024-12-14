using StoreManagement.Core.Interfaces.Formatting;
using System.Globalization;
using System.Text.Json;

namespace StoreManagement.Services.Formatting
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
                var integerPart = ((long)value).ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".");
                var fractionalPart = value % 1 == 0
                    ? string.Empty
                    : value.ToString("0.############################", CultureInfo.InvariantCulture)
                           .Split('.')[1]
                           .Replace(".", ",");

                return fractionalPart == string.Empty ? integerPart : $"{integerPart},{fractionalPart}";
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

        public T DeepCopyUsingJson<T>(T source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source object cannot be null.");
            }

            var serializedObject = JsonSerializer.Serialize(source);

            return JsonSerializer.Deserialize<T>(serializedObject);
        }
    }
}
