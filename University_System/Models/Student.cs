using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace University_System.Models
{
    public class Student : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string _firstName;
        [StringLength(30)] 
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        [StringLength(30)]
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        private string _gender;
        [StringLength(10)]
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }
        private string _email;
        [StringLength(30)]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;
        [StringLength(17)]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        [StringLength(20)]
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private DateTime _bornDateTime;
        public  DateTime BornDateTime
        {
            get => _bornDateTime;
            set
            {
                _bornDateTime = value;
                OnPropertyChanged();
            }
        }

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        Error = String.Empty;
        //        switch (columnName)
        //        {
        //            case "FirstName":
        //                if (!(Regex.IsMatch(FirstName, "^[a-zA-Z]{1,20}$")))
        //                {
        //                    Error = "Wrong first name!";

        //                }
        //                else
        //                {
        //                    Error = "";
        //                }
        //                break;
        //            case "LastName":
        //                if (!(Regex.IsMatch(LastName, "^[a-zA-Z]{1,20}$")))
        //                {

        //                    Error = "Wrong second name!";
        //                }
        //                else
        //                {
        //                    Error = "";
        //                }

        //                break;
        //            case "Age":
        //                if ((Age < 14) || (Age > 100))
        //                {
        //                    Error = "Wrong age!";
        //                }
        //                else
        //                {
        //                    Error = "";
        //                }

        //                break;
        //            case "Email":
        //                if (!(Regex.IsMatch(Email, @"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$")))
        //                {
        //                    Error = "Wrong email! Example: 'myemail@gmail.com'";
        //                }
        //                else
        //                {
        //                    Error = "";
        //                }

        //                break;
        //            case "PhoneNumber":
        //                if (!(Regex.IsMatch(PhoneNumber, @"^\+380\d\d[-]\d\d\d[-]\d\d[-]\d\d$")))
        //                {
        //                    Error = "Wrong phone number! Example: '+38011-111-11-11'";
        //                }
        //                else
        //                {
        //                    Error = "";
        //                }

        //                break;
        //            case "BornDateTime":
        //                if (!(Regex.IsMatch(BornDateTime.ToShortDateString(), @"^\d\d(\.|\/|\-)\d\d(\.|\/|\-)\d\d\d\d$")))
        //                {
        //                    Error = "Wrong date!";
        //                }
        //                int result = DateTime.Compare(BornDateTime, DateTime.Now);
        //                if (result >= 0)
        //                {
        //                    Error = "Wrong date!";

        //                }
        //                else
        //                {
        //                    Error = "";
        //                }

        //                break;
        //            default:
        //                Error = "";
        //                break;
        //        }

        //        return Error;
        //    }
        //}

        //private string _error;
        //[NotMapped]
        //public string Error
        //{
        //    get => _error;
        //    set
        //    {
        //        _error = value;
        //        OnPropertyChanged();
        //    }
        //}



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Sex
    {
        Male,
        Female
    }
}
