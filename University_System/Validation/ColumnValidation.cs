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

namespace University_System.Validation
{
    public class ColumnValidation: ValidationRule
    {
        private StudentHelper _studentList;

        public StudentHelper StudentList
        {
            get => _studentList;
            set => _studentList = value;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value == null)
                return ValidationResult.ValidResult;

            var student = (Student)((BindingGroup) value).Items[0];

            if (!(Regex.IsMatch(student.FirstName, "^[a-zA-Z]{1,20}$")))
            {
                return new ValidationResult(false, "Wrong first name!");
            }
            else if (!(Regex.IsMatch(student.LastName, "^[a-zA-Z]{1,20}$")))
            {
                return new ValidationResult(false, "Wrong second name!");
            }
            else if ((student.Age < 14) || (student.Age > 100))
            {
                return new ValidationResult(false, "Wrong age!");
            }
            else if (!(Regex.IsMatch(student.Email, @"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$")))
            {
                return new ValidationResult(false, "Wrong email! Example: 'myemail@gmail.com'!");
            }
            else if (!(Regex.IsMatch(student.PhoneNumber, @"^\+380\d\d[-]\d\d\d[-]\d\d[-]\d\d$")))
            {
                return new ValidationResult(false, "Wrong phone number! Example: '+38011-111-11-11'");
            }
            else if (!(Regex.IsMatch(student.BornDateTime.ToShortDateString(),
                @"^\d\d(\.|\/|\-)\d\d(\.|\/|\-)\d\d\d\d$")))
            {
                return new ValidationResult(false, "Wrong date");
            }

            return ValidationResult.ValidResult;

        }
    }
}
