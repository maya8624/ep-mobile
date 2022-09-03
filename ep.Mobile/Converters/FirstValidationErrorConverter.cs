using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ep.Mobile.Converters
{
    public class FirstValidationErrorConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
       value is ICollection<string> errors && errors.Count > 0
           ? errors.ElementAt(0)
           : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
