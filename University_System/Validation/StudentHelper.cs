using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using University_System.Models;
using University_System.ViewModel;

namespace University_System.Validation
{
    public class StudentHelper :DependencyObject
    {
        public StudentViewModel CurrentStudentViewModel
        {
            get => (StudentViewModel) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public  static  readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("CurrentStudentViewModel",
                typeof(StudentViewModel),
                typeof(StudentHelper),
                new UIPropertyMetadata(null));
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
    }
}
