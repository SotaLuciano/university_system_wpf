using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using University_System.Commands;
using University_System.Comparators;
using University_System.Models;

namespace University_System.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private IEnumerable _dataGridInformation;
        private bool _isEditModeEnabled;

        public bool IsEditModeEnabled
        {
            get => _isEditModeEnabled;
            set
            {
                _isEditModeEnabled = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable DataGridInformation
        {
            get => _dataGridInformation;
            set
            {
                _dataGridInformation = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Student> Students { get; set; }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Student>();
            AddButtonClick = new MyCommand(AddButton);
            LoadButtonClick = new MyCommand(LoadButton);
            EditButtonClick = new MyCommand(EditButton);
            CancelButtonClick = new MyCommand(CancelButton);
            SaveButtonClick = new MyCommand(SaveButton);
            //NewStudent = new Student()
            //{
            FirstName = "";
            LastName = "";
            Age = 0;
            Gender = "";
            Email = "";
            PhoneNumber = "+380";
            Address = "";
            BornDateTime = DateTime.Now;
            //};
            IsEditModeEnabled = false;

        }


        //private Student _newStudent;
        //public Student NewStudent
        //{
        //    get => _newStudent;
        //    set
        //    {
        //        _newStudent = value;
        //        OnPropertyChanged();
        //    }
        //}
        //Property -> Student?
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime BornDateTime { get; set; }

        public ICommand AddButtonClick { get; }
        public ICommand LoadButtonClick { get; }
        public ICommand SaveButtonClickAddNewStudentWindow { get; set; }
        public ICommand EditButtonClick { get; set; }
        public ICommand CancelButtonClick { get; set; }
        public ICommand SaveButtonClick { get; set; }

        private void AddButton()
        {
            var window = new AddNewStudentWindow(this);
            if (window.ShowDialog() == true)
            {
                var newStudent = new Student()
                {
                    Address = Address,
                    Age = Age,
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    Gender = Gender,
                    PhoneNumber = PhoneNumber,
                    BornDateTime = BornDateTime
                };

                using (var db = new StudentContext())
                {
                    try
                    {
                        db.Students.Add(newStudent);
                        db.SaveChanges();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString(), "Error!");
                        throw;
                    }
                }
            }
        }

        private void LoadButton()
        {
            Students.Clear();
            DataGridInformation = Students;

            using (var db = new StudentContext())
            {
                db.Students.Load();
                var studs = db.Students.ToList();

                foreach (var stud in studs)
                {
                    Students.Add(stud);
                }
            }

        }

        private void SaveButton()
        {
            using (var db = new StudentContext())
            {
                db.Students.Load();
                var studs = db.Students.ToList();

                foreach (var stud in studs)
                {
                    var curStud = Students.FirstOrDefault((x) => x.Id == stud.Id);
                    if(curStud == null)
                        continue;

                    if (!StudentComparator.CompareStudents(stud, curStud))
                    {
                        stud.FirstName = curStud.FirstName;
                        stud.LastName = curStud.LastName;
                        stud.Age = curStud.Age;
                        stud.Gender = curStud.Gender;
                        stud.Address = curStud.Address;
                        stud.Email = curStud.Email;
                        stud.BornDateTime = curStud.BornDateTime;
                        db.SaveChanges();
                    }

                }
            }
            IsEditModeEnabled = false;
            LoadButton();
        }

        private void EditButton()
        {
            IsEditModeEnabled = true;
        }

        private void CancelButton()
        {
            LoadButton();
            IsEditModeEnabled = false;
        }

        public string this[string columnName]
        {
            get
            {
                Error = String.Empty;
                switch (columnName)
                {
                    case "FirstName":
                        if (!(Regex.IsMatch(FirstName, "^[a-zA-Z]{1,20}$")))
                        {
                            Error = "Wrong first name!"; 

                        }
                        else
                        {
                            Error = "";
                        }
                        break;
                    case "LastName":
                        if (!(Regex.IsMatch(LastName, "^[a-zA-Z]{1,20}$")))
                        {

                            Error = "Wrong second name!";
                        }
                        else
                        {
                            Error = "";
                        }

                        break;
                    case "Age":
                        if ((Age < 14) || (Age > 100))
                        {
                            Error = "Wrong age!";
                        }
                        else
                        {
                            Error = "";
                        }

                        break;
                    case "Email":
                        if (!(Regex.IsMatch(Email, @"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$")))
                        {
                            Error = "Wrong email! Example: 'myemail@gmail.com'";
                        }
                        else
                        {
                            Error = "";
                        }

                        break;
                    case "PhoneNumber":
                        if (!(Regex.IsMatch(PhoneNumber, @"^\+380\d\d[-]\d\d\d[-]\d\d[-]\d\d$")))
                        {
                            Error = "Wrong phone number! Example: '+38011-111-11-11'";
                        }
                        else
                        {
                            Error = "";
                        }

                        break;
                    case "BornDateTime":
                        if (!(Regex.IsMatch(BornDateTime.ToShortDateString(), @"^\d\d(\.|\/|\-)\d\d(\.|\/|\-)\d\d\d\d$")))
                        {
                            Error = "Wrong date!";
                        }
                        int result = DateTime.Compare(BornDateTime, DateTime.Now);
                        if (result >= 0)
                        {
                            Error = "Wrong date!";

                        }                        else
                        {
                            Error = "";
                        }

                        break;
                    default:
                        Error = "";
                        break;
                }
                    
                return Error;
            }
        }

        private string _error;
        public string Error
        {
            get => _error;
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
