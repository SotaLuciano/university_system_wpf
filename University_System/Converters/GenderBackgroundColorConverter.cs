using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using University_System.Models;
using University_System.ViewModel;

namespace University_System.Converters
{
    public class GenderBackgroundColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return new SolidColorBrush(Colors.Transparent);

            if (values[1] is DataGridCell dataGridCell && dataGridCell.Column.Header != null && values[0] != null)
            {
                if (dataGridCell.Column.Header.ToString() == "Gender")
                {
                    switch (values[0])
                    {
                        case "Male":
                            return new SolidColorBrush(Colors.Red);
                        case "Female":
                            return new SolidColorBrush(Colors.Blue);
                    }
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
