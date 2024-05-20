using System.Globalization;

namespace QrCodeReaderApp
{
    public class StringIsUrlConverter : IValueConverter
    {
        public bool IsValidUri (string? value) => Uri.TryCreate(value, UriKind.Absolute, out var r)
              && (r.Scheme == Uri.UriSchemeHttp || r.Scheme == Uri.UriSchemeHttps);

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
                return IsValidUri(str);
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
