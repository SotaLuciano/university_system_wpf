using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using University_System.Models;

namespace University_System.Validation
{
    public class StudentHelper :DependencyObject
    {
        public ObservableCollection<Student> Students
        {
            get => (ObservableCollection<Student>) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public  static  readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items",
                typeof(ObservableCollection<Student>),
                typeof(StudentHelper),
                new UIPropertyMetadata(null));
    }
}
