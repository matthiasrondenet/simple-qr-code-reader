using System.Globalization;
using ZXing.Net.Maui;

namespace QrCodeReaderApp
{
    public class BarcodeDetectionEventArgsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not BarcodeDetectionEventArgs barcodeDetectionEventArgs)
                return null;
            return barcodeDetectionEventArgs.Results;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
