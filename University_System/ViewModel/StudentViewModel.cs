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
using Group = System.Text.RegularExpressions.Group;

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
        public  ObservableCollection<AdministrativeInformation> AdministrativeInformations { get; set; }

        private AdministrativeInformation _selectedRowAdministrativeInformation;

        public AdministrativeInformation SelectedRowAdministrativeInformation
        {
            get => _selectedRowAdministrativeInformation;
            set
            {
                _selectedRowAdministrativeInformation = value;
                OnPropertyChanged();
                SelectedGroupChanged();
            }
        }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Student>();
            AdministrativeInformations = new ObservableCollection<AdministrativeInformation>();

            AddButtonClick = new MyCommand(AddButton);
            LoadButtonClick = new MyCommand(LoadButton);
            EditButtonClick = new MyCommand(EditButton);
            CancelButtonClick = new MyCommand(CancelButton);
            SaveButtonClick = new MyCommand(SaveButton);
            RemoveStudentButtonClick = new CommandWithParameter<object>(RemoveStudentButton);
            RemoveGroupButtonClick = new CommandWithParameter<object>(RemoveGroupButton);
            AddStudentToGroupButtonClick = new CommandWithParameter<object>(AddStudentToGroupButton);
            DuplicateStudentButtonClick = new CommandWithParameter<object>(DuplicateStudentButton);

            FirstName = "";
            LastName = "";
            Age = 0;
            Gender = "";
            Email = "";
            PhoneNumber = "+380";
            Address = "";
            BornDateTime = DateTime.Now;
            GroupId = 0;

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
        public int GroupId { get; set; }

        public ICommand AddButtonClick { get; }
        public ICommand LoadButtonClick { get; }
        public ICommand RemoveStudentButtonClick { get; }
        public ICommand SaveButtonClickAddNewStudentWindow { get; set; }
        public ICommand EditButtonClick { get; set; }
        public ICommand CancelButtonClick { get; set; }
        public ICommand SaveButtonClick { get; set; }
        public ICommand RemoveGroupButtonClick { get; }
        public ICommand AddStudentToGroupButtonClick { get; }
        public ICommand DuplicateStudentButtonClick { get; }

        
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
                    BornDateTime = BornDateTime,
                    GroupId = GroupId
                };

                using (var db = new StudentContext())
                {
                    db.Groups.Load();
                    var group = db.Groups.FirstOrDefault(x => x.Id == newStudent.GroupId);
                    if (group == null)
                    {
                        MessageBox.Show("Wrong GroupId!", "Error!");
                        return;
                    }
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
            AdministrativeInformations.Clear();
            DataGridInformation = Students;

            using (var db = new StudentContext())
            {
                var admInfo = from g in db.Groups
                    join s in db.Specializations on g.SpecializationId equals s.Id
                    join d in db.Departments on s.DepartmentId equals d.Id
                    join i in db.Institutes on d.InstituteId equals i.Id
                    select new
                    {
                        GroupId = g.Id,
                        GroupName = g.Name,
                        SpecializationName = s.Name,
                        DepartmentName = d.Name,
                        InstituteName = i.Name
                    };

                db.Students.Load();
                var studs = db.Students.ToList();

                foreach (var info in admInfo)
                {
                    AdministrativeInformations.Add(new AdministrativeInformation()
                    {
                        GroupId = info.GroupId,
                        GroupName = info.GroupName,
                        SpecializationName = info.SpecializationName,
                        DepartmentName = info.DepartmentName,
                        InstituteName = info.InstituteName
                    });
                }
            }

        }

        private async void RemoveStudentButton(object parameter)
        {
            var menuItem = (MenuItem) parameter;

            if (menuItem.Parent is ContextMenu contextMenu)
            {
                if (contextMenu.PlacementTarget is DataGrid dataGrid)
                {
                    if (dataGrid.SelectedItem is Student selectedStudent)
                    {
                        var result = MessageBox.Show("Are you sure that you would like to remove this student?",
                            "Remove student!",
                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            using (var db = new StudentContext())
                            {
                                var student = await db.Students.FindAsync(selectedStudent.Id);

                                if (student != null)
                                {
                                    db.Students.Remove(student);
                                    await db.SaveChangesAsync();
                                    SelectedGroupChanged();
                                    LoadButton();
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void RemoveGroupButton(object parameter)
        {
            var menuItem = (MenuItem)parameter;

            if (menuItem.Parent is ContextMenu contextMenu)
            {
                if (contextMenu.PlacementTarget is DataGrid dataGrid)
                {
                    if (dataGrid.SelectedItem is AdministrativeInformation selectedAdministrativeInformation)
                    {
                        var result = MessageBox.Show("Are you sure that you would like to remove this group?",
                            "Remove group!",
                            MessageBoxButton.YesNo, 
                            MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            using (var db = new StudentContext())
                            {
                                db.Groups.Load();
                                var group = await db.Groups.FirstOrDefaultAsync(x => x.Id == selectedAdministrativeInformation.GroupId);
                                if (group != null)
                                {
                                    db.Groups.Remove(group);
                                    await db.SaveChangesAsync();
                                    LoadButton();
                                    Students.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddStudentToGroupButton(object parameter)
        {
            var menuItem = (MenuItem)parameter;

            if (menuItem.Parent is ContextMenu contextMenu)
            {
                if (contextMenu.PlacementTarget is DataGrid dataGrid)
                {
                    if (dataGrid.SelectedItem is AdministrativeInformation selectedAdministrativeInformation)
                    {
                        FirstName = "";
                        LastName = "";
                        Age = 0;
                        Gender = "";
                        Email = "";
                        PhoneNumber = "+380";
                        Address = "";
                        BornDateTime = DateTime.Now;
                        GroupId = selectedAdministrativeInformation.GroupId;
                        AddButton();
                        SelectedGroupChanged();
                    }
                }
            }
        }

        private void DuplicateStudentButton(object parameter)
        {
            var menuItem = (MenuItem)parameter;

            if (menuItem.Parent is ContextMenu contextMenu)
            {
                if (contextMenu.PlacementTarget is DataGrid dataGrid)
                {
                    if (dataGrid.SelectedItem is Student selectedStudent)
                    {
                        using (var db = new StudentContext())
                        {
                            var student = new Student()
                            {
                                FirstName = selectedStudent.FirstName,
                                LastName = selectedStudent.LastName,
                                Age = selectedStudent.Age,
                                Gender = selectedStudent.Gender,
                                Address = selectedStudent.Address,
                                Email = selectedStudent.Email,
                                PhoneNumber = selectedStudent.PhoneNumber,
                                BornDateTime = selectedStudent.BornDateTime,
                                GroupId = selectedStudent.GroupId
                            };

                            db.Students.Add(student);
                            db.SaveChanges();
                            SelectedGroupChanged();
                        }
                    }
                }
            }
        }

        private void SelectedGroupChanged()
        {
            if(SelectedRowAdministrativeInformation == null)
                return;
            using (var db = new StudentContext())
            {
                Students.Clear();
                db.Students.Load();
                var students = db.Students.Where(x => x.GroupId == SelectedRowAdministrativeInformation.GroupId);

                foreach (var student in students)
                {
                    Students.Add(student);
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
                        break;
                    case "LastName":
                        if (!(Regex.IsMatch(LastName, "^[a-zA-Z]{1,20}$")))
                        {

                            Error = "Wrong second name!";
                        }
                        break;
                    case "Age":
                        if ((Age < 14) || (Age > 100))
                        {
                            Error = "Wrong age!";
                        }
                        break;
                    case "Email":
                        if (!(Regex.IsMatch(Email, @"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$")))
                        {
                            Error = "Wrong email! Example: 'myemail@gmail.com'";
                        }
                        break;
                    case "PhoneNumber":
                        if (!(Regex.IsMatch(PhoneNumber, @"^\+380\d\d[-]\d\d\d[-]\d\d[-]\d\d$")))
                        {
                            Error = "Wrong phone number! Example: '+38011-111-11-11'";
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
