using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using University_System.Models;
using University_System.ViewModel;

namespace University_System.Validation
{
    public class ColumnValidation: ValidationRule
    {
        private StudentHelper _currentStudentViewModel;

        public StudentHelper CurrentStudentViewModel
        {
            get => _currentStudentViewModel;
            set => _currentStudentViewModel = value;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = "";
            if (value == null)
            {
                return ValidationResult.ValidResult;
            }

            var student = (Student)((BindingGroup) value).Items[0];

            if (student.FirstName != null && !(Regex.IsMatch(student.FirstName, "^[a-zA-Z]{1,20}$")))
            {
                error = "Wrong first name!";
            }
            else if (student.LastName != null && !(Regex.IsMatch(student.LastName, "^[a-zA-Z]{1,20}$")))
            {
                error = "Wrong last name!";
            }
            else if (((student.Age < 14) || (student.Age > 100)))
            {
                error = "Wrong age!";
            }
            else if (student.Email != null && !(Regex.IsMatch(student.Email, @"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$")))
            {
                error = "Wrong email! Example: 'myemail@gmail.com'!";
            }
            else if (student.PhoneNumber != null && !(Regex.IsMatch(student.PhoneNumber, @"^\+380\d\d[-]\d\d\d[-]\d\d[-]\d\d$")))
            {
                error = "Wrong phone number! Example: '+38011-111-11-11'";
            }
            else if (!(Regex.IsMatch(student.BornDateTime.ToShortDateString(),
                @"^\d\d(\.|\/|\-)\d\d(\.|\/|\-)\d\d\d\d$")))
            {
                error = "Wrong date!";
            }
            int result = DateTime.Compare(student.BornDateTime, DateTime.Now);

            if (result >= 0)
            {
                error = "Wrong date.";
            }
            result = DateTime.Compare(student.BornDateTime, new DateTime(1900, 1, 1));

            if (result <= 0)
            {
                error = "Wrong date";
            }

            if (error == "")
            {
                CurrentStudentViewModel.CurrentStudentViewModel.IsSaveEnable = true;
                return ValidationResult.ValidResult;
            }
            else
            {
                CurrentStudentViewModel.CurrentStudentViewModel.IsSaveEnable = false;
                return new ValidationResult(false, error);
            }
        }
    }
}
