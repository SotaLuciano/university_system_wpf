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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using University_System.Commands;
using University_System.Comparators;
using University_System.Models;
using Group = System.Text.RegularExpressions.Group;

namespace University_System.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
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

        // Item source for InfoGrid.
        private IEnumerable _dataGridInformation;
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

        // ItemSource for Combobox 'all groups'.
        public ObservableCollection<AdministrativeInformation> AllAdministrativeInformations { get; set; }

        // ItemSource for group data grid.
        public ObservableCollection<AdministrativeInformation> CurrentAdministrativeInformations { get; set; }
        
        // Selected row in left(groups) grid.
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

        // DoubleClick popup status.
        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }
        // DoubleClick popup info.
        private string _popupText;
        public string PopupText
        {
            get => _popupText;
            set
            {
                _popupText = value;
                OnPropertyChanged();
            }
        }
        // DoubleClick popup placement.
        private PlacementMode _popupPlacementMode;
        public PlacementMode PopupPlacementMode
        {
            get => _popupPlacementMode;
            set
            {
                _popupPlacementMode = value;
                OnPropertyChanged();
            }
        }

        // Filters
        private string _lastNameFilter;
        public  string LastNameFilter
        {
            get => _lastNameFilter;
            set
            {
                _lastNameFilter = value;
                OnPropertyChanged();
            }
        }

        private int _genderFilter;
        public int GenderFilter
        {
            get => _genderFilter;
            set
            {
                _genderFilter = value;
                OnPropertyChanged();
            }
        }

        private DateTime _fromDateFilter;
        public DateTime FromDateFilter
        {
            get => _fromDateFilter;
            set
            {
                _fromDateFilter = value;
                IsFromDateFilterEnable = true;
                OnPropertyChanged();
            }
        }
        public bool IsFromDateFilterEnable;

        private DateTime _toDateFilter;
        public DateTime ToDateFilter
        {
            get => _toDateFilter;
            set
            {
                _toDateFilter = value;
                IsToDateFilterEnable = true;
                OnPropertyChanged();
            }
        }
        public bool IsToDateFilterEnable;

        private bool _isDatePickerEnable;
        public bool IsDatePickerEnable
        {
            get => _isDatePickerEnable;
            set { _isDatePickerEnable = value;
                OnPropertyChanged(); }
        }

        // Text on the button - 'Enable' - 'Disable'.
        private string _isDatePickerEnableText;
        public string IsDatePickerEnableText
        {
            get => _isDatePickerEnableText;
            set
            {
                _isDatePickerEnableText = value;
                OnPropertyChanged();
            }
        }

        public StudentViewModel()
        {
            // Setting initial data.

            Students = new ObservableCollection<Student>();
            AllAdministrativeInformations = new ObservableCollection<AdministrativeInformation>();
            CurrentAdministrativeInformations = new ObservableCollection<AdministrativeInformation>();

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
            GenderFilter = 0;
            IsDatePickerEnableText = "Enable";
            IsDatePickerEnable = false;
            IsFromDateFilterEnable = false;
            IsToDateFilterEnable = false;
        }

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
            // Creating new window.
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
                    // Finding group.
                    var group = db.Groups.FirstOrDefault(x => x.Id == newStudent.GroupId);
                    if (group == null)
                    {
                        MessageBox.Show("Wrong GroupId!", "Error!");
                        return;
                    }
                    // Add new student.
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
            AllAdministrativeInformations.Clear();
            DataGridInformation = Students;

            using (var db = new StudentContext())
            {
                // Select all groups with specialization, department and institute information.
                var admInfo = from g in db.Groups
                    join s in db.Specializations on g.SpecializationId equals s.Id
                    join d in db.Departments on s.DepartmentId equals d.Id
                    join i in db.Institutes on d.InstituteId equals i.Id
                    select new
                    {
                        GroupId = g.Id,
                        GroupName = g.Name,
                        SpecializationId = s.Id,
                        SpecializationName = s.Name,
                        DepartmentId = d.Id,
                        DepartmentName = d.Name,
                        InstituteId = i.Id,
                        InstituteName = i.Name
                    };
                // Add group to combobox itemsource.
                foreach (var info in admInfo)
                {
                    AllAdministrativeInformations.Add(new AdministrativeInformation()
                    {
                        GroupId = info.GroupId,
                        GroupName = info.GroupName,
                        SpecializationId = info.SpecializationId,
                        SpecializationName = info.SpecializationName,
                        DepartmentId = info.DepartmentId,
                        DepartmentName = info.DepartmentName,
                        InstituteId = info.InstituteId,
                        InstituteName = info.InstituteName
                    });
                }
            }

        }

        private async void RemoveStudentButton(object parameter)
        {
            // Getting selected student.
            if (parameter is MenuItem menuItem 
                && menuItem.Parent is ContextMenu contextMenu
                && contextMenu.PlacementTarget is DataGrid dataGrid
                && dataGrid.SelectedItem != null
                && dataGrid.SelectedItem is Student selectedStudent)
            {
                var result = MessageBox.Show("Are you sure that you would like to remove this student?",
                    "Remove student!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new StudentContext())
                    {
                        // Find student in db.
                        var student = await db.Students.FindAsync(selectedStudent.Id);

                        if (student != null)
                        {
                            // Remove student if exist.
                            db.Students.Remove(student);
                            await db.SaveChangesAsync();
                            SelectedGroupChanged();
                            LoadButton();
                        }
                    }
                }     
            }
        }

        private async void RemoveGroupButton(object parameter)
        {
            // Getting selected group.
            if (parameter is MenuItem menuItem
                && menuItem.Parent is ContextMenu contextMenu
                && contextMenu.PlacementTarget is DataGrid dataGrid
                && dataGrid.SelectedItem is AdministrativeInformation selectedAdministrativeInformation)
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
                        // Finding group.
                        var group = await db.Groups.FirstOrDefaultAsync(x => x.Id == selectedAdministrativeInformation.GroupId);
                        if (group != null)
                        {
                            // Remove group if exist.
                            db.Groups.Remove(group);
                            await db.SaveChangesAsync();
                            LoadButton();
                            Students.Clear();
                        }
                    }
                }           
            }
        }

        private void AddStudentToGroupButton(object parameter)
        {

            if (parameter is MenuItem menuItem
                && menuItem.Parent is ContextMenu contextMenu
                && contextMenu.PlacementTarget is DataGrid dataGrid
                && dataGrid.SelectedItem is AdministrativeInformation selectedAdministrativeInformation)
            {
                // Set initial data.
                FirstName = "";
                LastName = "";
                Age = 0;
                Gender = "";
                Email = "";
                PhoneNumber = "+380";
                Address = "";
                BornDateTime = DateTime.Now;
                GroupId = selectedAdministrativeInformation.GroupId;

                // Invoke 'add student' logic.
                AddButton();
                SelectedGroupChanged();            
            }
        }

        private void DuplicateStudentButton(object parameter)
        {
            // Getting selected student.
            if (parameter is MenuItem menuItem
                && menuItem.Parent is ContextMenu contextMenu
                && contextMenu.PlacementTarget is DataGrid dataGrid
                && dataGrid.SelectedItem is Student selectedStudent)
            {
                var window = new AddNewStudentWindow(this);
                // Copy student information.
                Address = selectedStudent.Address;
                Age = selectedStudent.Age;
                Email = selectedStudent.Email;
                FirstName = selectedStudent.FirstName;
                LastName = selectedStudent.LastName;
                Gender = selectedStudent.Gender;
                PhoneNumber = selectedStudent.PhoneNumber;
                BornDateTime = selectedStudent.BornDateTime;
                GroupId = selectedStudent.GroupId;

                // Open 'add student' window.
                if (window.ShowDialog() == true)
                {
                    // Add new student.
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

        private void SelectedGroupChanged()
        {
            if(SelectedRowAdministrativeInformation == null)
                return;
            using (var db = new StudentContext())
            {
                Students.Clear();
                db.Students.Load();

                // Get all students with selected groupId.
                var students = db.Students.Where(x => x.GroupId == SelectedRowAdministrativeInformation.GroupId);

                // Add this student to the ObservableCollection.
                foreach (var student in students)
                {
                    Students.Add(student);
                }

                // Clear all filters.
                LastNameFilter = "";
                GenderFilter = 0;
                IsDatePickerEnableText = "Enable";
                IsDatePickerEnable = false;
                IsFromDateFilterEnable = false;
                IsToDateFilterEnable = false;
                IsEditModeEnabled = false;

                // Set ItemSource.
                DataGridInformation = Students;
            }
        }

        private void SaveButton()
        {
            using (var db = new StudentContext())
            {
                db.Students.Load();

                // Find each student from datagrid in db.
                foreach (var curStud in Students)
                {
                    var student = db.Students.FirstOrDefault(x => x.Id == curStud.Id);
                    // If there is no student with this Id => add this student to db.
                    if (student == null)
                    {
                        db.Students.Add(curStud);
                        db.SaveChanges();
                        continue;
                    }
                    // Check for changes in student.
                    if (!StudentComparator.CompareStudents(student, curStud))
                    {
                        student.FirstName = curStud.FirstName;
                        student.LastName = curStud.LastName;
                        student.Age = curStud.Age;
                        student.Gender = curStud.Gender;
                        student.Address = curStud.Address;
                        student.Email = curStud.Email;
                        student.BornDateTime = curStud.BornDateTime;
                        student.GroupId = curStud.GroupId;
                        db.SaveChanges();
                    }
                }

                db.Groups.Load();

                // Change group name in db.
                foreach (var admInfo in CurrentAdministrativeInformations)
                {
                    var group = db.Groups.FirstOrDefault(x => x.Id == admInfo.GroupId);
                    // If there is difference between names -> change db.
                    if (group != null && group.Name != admInfo.GroupName)
                    {
                        group.Name = admInfo.GroupName;
                        db.SaveChanges();
                    }
                }
            }
            
            // Turn off edit mode and reload grid.
            IsEditModeEnabled = false;
            LoadButton();
        }

        private void EditButton()
        {
            // Turn on edit mode.
            IsEditModeEnabled = true;
        }

        private void CancelButton()
        {
            if (Students.Count > 0)
            {
                using (var db = new StudentContext())
                {
                    db.Students.Load();

                    // If students in datagrid more than in db - remove last.
                    var dbStudents = db.Students.ToList().Where(x => x.GroupId == Students[0].GroupId);
                    int amountToRemove = Students.Count - dbStudents.Count();

                    if (amountToRemove > 0)
                    {
                        for (int i = 0; i < amountToRemove; i++)
                        {
                            Students.RemoveAt(Students.Count - 1);
                        }

                    }
                }

            }
            // Turn off edit mode and reload grid.
            LoadButton();
            IsEditModeEnabled = false;
        }

        // Data Validation.
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

                        result = DateTime.Compare(BornDateTime, new DateTime(1900, 1, 1));
                        if (result <= 0)
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