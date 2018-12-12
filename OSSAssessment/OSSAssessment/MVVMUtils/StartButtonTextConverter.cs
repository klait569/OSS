using System;
using System.Globalization;
using System.Windows.Data;

namespace OSSAssessment.MVVMUtils
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class StartButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Do the conversion from bool to visibility
            if ((bool)value)
            {
                return "Stop sharing";
            }
            else
            {
                return "Start sharing";
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StartButtonTextConverter");
        }
    }
}