using System;
using System.Globalization;
using System.Windows.Data;

namespace Reolmarked.Helper
{
    public class NegativeToTrueConverter : IValueConverter
    {
        /*
         Flere steder skal vi vise forskellige farver baseret på beløbets størrelse osv.

         Vores løsning blev for kringlet og for kompliceret, så derfor har vi den her helper class, som blev udarbejdet af AI.
        */

        public static NegativeToTrueConverter Instance { get; } = new NegativeToTrueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue < 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PositiveToTrueConverter : IValueConverter
    {
        public static PositiveToTrueConverter Instance { get; } = new PositiveToTrueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}