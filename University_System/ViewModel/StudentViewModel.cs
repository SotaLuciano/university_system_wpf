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

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged();
            }

        }

        private bool _isSaveEnable;

        public bool IsSaveEnable
        {
            get => _isSaveEnable;
            set
            {
                _isSaveEnable = value;
                OnPropertyChanged();
                SaveButtonClick.CanExecute(null);
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
        public ObservableCollection<AdministrativeInformation> CurrentAdministrativeInformationsInDataGrid { get; set; }
        public ObservableCollection<AdministrativeInformation> CurrentAdministrativeInformationsInComboBox { get; set; }

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
                FilterHandler();
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
                FilterHandler();
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
                FilterHandler();
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
                FilterHandler();
            }
        }
        public bool IsToDateFilterEnable;

        private bool _isDatePickerEnable;
        public bool IsDatePickerEnable
        {
            get => _isDatePickerEnable;
            set
            {
                _isDatePickerEnable = value;
                OnPropertyChanged();
                FilterHandler();
            }
        }

        private string _groupNameFilter;
        public string GroupNameFilter
        {
            get => _groupNameFilter;
            set
            {
                _groupNameFilter = value;
                OnPropertyChanged();
                UseComboboxGroupFilter();
            }
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

        private DataGridCellInfo _copiedDataGridCell;
        public DataGridCellInfo CopiedDataGridCell
        {
            get => _copiedDataGridCell;
            set
            {
                _copiedDataGridCell = value;
                OnPropertyChanged();
            }
        }

        private Student _copiedDataGridRow;
        public Student CopiedDataGridRow
        {
            get => _copiedDataGridRow;
            set
            {
                _copiedDataGridRow = value;
                OnPropertyChanged();
            }
        }

        private string _selectedGroupsNames;
        public string SelectedGroupsNames
        {
            get => _selectedGroupsNames;
            set
            {
                _selectedGroupsNames = value;
                OnPropertyChanged();
            }
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
        public ICommand RemoveStudentContextButtonClick { get; }
        public ICommand SaveButtonClickAddNewStudentWindow { get; set; }
        public ICommand EditButtonClick { get; set; }
        public ICommand CancelButtonClick { get; set; }
        public ICommand SaveButtonClick { get; set; }
        public ICommand RemoveGroupContextButtonClick { get; }
        public ICommand AddStudentToGroupContextButtonClick { get; }
        public ICommand DuplicateStudentContextButtonClick { get; }
        public ICommand CopyCellContextButtonClick { get; }
        public ICommand PasteCellContextButtonClick { get; }
        public ICommand CopyRowContextButtonClick { get; }
        public ICommand PasteRowContextButtonClick { get; }
        public StudentViewModel()
        {
            // Setting initial data.

            Students = new ObservableCollection<Student>();
            AllAdministrativeInformations = new ObservableCollection<AdministrativeInformation>();
            CurrentAdministrativeInformationsInDataGrid = new ObservableCollection<AdministrativeInformation>();
            CurrentAdministrativeInformationsInComboBox = new ObservableCollection<AdministrativeInformation>();

            AddButtonClick = new MyCommand(AddButton);
            LoadButtonClick = new MyCommand(LoadButton);
            EditButtonClick = new MyCommand(EditButton);
            CancelButtonClick = new MyCommand(CancelButton);
            SaveButtonClick = new MyCommand(SaveButton, ()=>IsSaveEnable);
            RemoveStudentContextButtonClick = new CommandWithParameter<object>(RemoveStudentContextButton);
            RemoveGroupContextButtonClick = new CommandWithParameter<object>(RemoveGroupContextButton);
            AddStudentToGroupContextButtonClick = new CommandWithParameter<object>(AddStudentToGroupContextButton);
            DuplicateStudentContextButtonClick = new CommandWithParameter<object>(DuplicateStudentContextButton);
            CopyCellContextButtonClick = new CommandWithParameter<object>(CopyCellContextButton);
            PasteCellContextButtonClick = new CommandWithParameter<object>(PasteCellContextButton);
            CopyRowContextButtonClick = new CommandWithParameter<object>(CopyRowContextButton);
            PasteRowContextButtonClick = new CommandWithParameter<object>(PasteRowContextButton);

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
            GroupNameFilter = "";

            IsDataLoaded = false;
            IsSaveEnable = true;
        }

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
            CurrentAdministrativeInformationsInComboBox.Clear();
            CurrentAdministrativeInformationsInDataGrid.Clear();
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
                        InstituteName = info.InstituteName,
                        IsSelected =  false
                    });
                }
            }
            // Set combobox enable.
            IsDataLoaded = true;
            // Clear group filter.
            GroupNameFilter = "";
        }

        private async void RemoveStudentContextButton(object parameter)
        {
            // Getting selected student.
            if (parameter is Student selectedStudent)
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
                        }
                    }
                }     
            }
        }

        private async void RemoveGroupContextButton(object parameter)
        {
            // Getting selected group.
            if (parameter is AdministrativeInformation selectedAdministrativeInformation)
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

        private void AddStudentToGroupContextButton(object parameter)
        {
            if (parameter is Student selectedStudent)
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
                GroupId = selectedStudent.GroupId;

                // Invoke 'add student' logic.
                AddButton();
                SelectedGroupChanged();            
            }
        }

        private void DuplicateStudentContextButton(object parameter)
        {
            // Getting selected student.
            if (parameter is Student selectedStudent)
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

        private void CopyCellContextButton(object parameter)
        {
            if (parameter is DataGrid dataGrid)
            {
                // Save cell.
                CopiedDataGridCell = dataGrid.CurrentCell;

                // Save value to ctrl + c buffer.
                Clipboard.SetData(DataFormats.Text, (CopiedDataGridCell.Item as Student).GetType().GetProperty(CopiedDataGridCell.Column.Header.ToString())
                    .GetValue((CopiedDataGridCell.Item as Student)));
            }
        }

        private void PasteCellContextButton(object parameter)
        {
            if (parameter is DataGrid dataGrid)
            {
                var currentCell = dataGrid.CurrentCell;

                // Check headers.
                if (CopiedDataGridCell.Column.Header.ToString() == currentCell.Column.Header.ToString())
                {
                    if (currentCell.Item is Student currentStudent)
                    {
                        // Find current student.
                        var student = Students.FirstOrDefault(x => x.Id == currentStudent.Id);

                        // Change value in current student if exist.
                        if (student != null)
                        {
                            student.GetType().GetProperty(CopiedDataGridCell.Column.Header.ToString())
                                .SetValue(student, (CopiedDataGridCell.Item as Student).GetType().GetProperty(CopiedDataGridCell.Column.Header.ToString())
                                    .GetValue((CopiedDataGridCell.Item as Student)));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Wrong column! You can paste it only in '" + CopiedDataGridCell.Column.Header.ToString() + "' column!"
                                    , "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CopyRowContextButton(object parameter)
        {
            if (parameter is Student student)
            {
                CopiedDataGridRow = student;

                string info = "First name: " + student.FirstName + "\n Last name: " + student.LastName + "\n Age: " 
                              + student.Age + "\n Gender: " + student.Gender + "\n Email: " + student.Email 
                              + "\n Phone number: " + student.PhoneNumber + "\n Address: " + student.Address 
                              + "\n Born: " + student.BornDateTime.ToShortDateString();
                Clipboard.SetData(DataFormats.Text, info);
            }
        }

        private void PasteRowContextButton(object parameter)
        {
            if (parameter is DataGrid dataGrid)
            {
                var currentRow = dataGrid.SelectedItem as Student;

                if (currentRow != null)
                {
                    var student = Students.FirstOrDefault(x => x.Id == currentRow.Id);
                    if (student != null)
                    {
                        student.FirstName = CopiedDataGridRow.FirstName;
                        student.LastName = CopiedDataGridRow.LastName;
                        student.Age = CopiedDataGridRow.Age;
                        student.Gender = CopiedDataGridRow.Gender;
                        student.Email = CopiedDataGridRow.Email;
                        student.PhoneNumber = CopiedDataGridRow.PhoneNumber;
                        student.Address = CopiedDataGridRow.Address;
                        student.BornDateTime = CopiedDataGridRow.BornDateTime;
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
                foreach (var admInfo in CurrentAdministrativeInformationsInDataGrid)
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
            SelectedGroupChanged();
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
            SelectedGroupChanged();
            IsEditModeEnabled = false;
        }

        private string GetGender(int index)
        {
            // Get gender string from filter.
            string gender = "";
            switch (index)
            {
                case 0:
                    gender = "None";
                    break;
                case 1:
                    gender = "Male";
                    break;
                case 2:
                    gender = "Female";
                    break;
            }

            return gender;
        }

        private void FilterHandler()
        {
            // One handler for all filters.
            IEnumerable<Student> result = UseFilter(LastNameFilter, GetGender(GenderFilter), IsFromDateFilterEnable,
                                                    FromDateFilter, IsToDateFilterEnable, ToDateFilter, Students);

            // Write info to ObservableCollection.
            ObservableCollection<Student> observableResult = new ObservableCollection<Student>();
            foreach (var r in result)
            {
                observableResult.Add(r);
            }
            DataGridInformation = observableResult;
        }

        private IEnumerable<Student> UseFilter(string lastName, string gender, bool isFromDateSet, DateTime fromDate, bool isToDateSet, DateTime toDate, IEnumerable<Student> studentsList)
        {
            // Using lastname filter.
            var lastNameFilterResult = studentsList.Where(x => x.LastName.ToLower().Contains(lastName.ToLower()));
            var genderFilterResult = lastNameFilterResult;

            // Using gender filter.
            if (gender != "None")
            {
                genderFilterResult = lastNameFilterResult.Where(x => x.Gender == gender);
            }

            var fromDateFilterResult = genderFilterResult;
            // Using fromDate filter.
            if (isFromDateSet)
            {
                fromDateFilterResult = genderFilterResult.Where(x => DateTime.Compare(x.BornDateTime, fromDate) > 0);
            }

            var toDateFilterResult = fromDateFilterResult;
            // Using toDate filter. 
            if (isToDateSet)
            {
                toDateFilterResult = toDateFilterResult.Where(x => DateTime.Compare(x.BornDateTime, toDate) < 0);
            }

            return toDateFilterResult;
        }

        private void UseComboboxGroupFilter()
        {
            CurrentAdministrativeInformationsInComboBox.Clear();
            if (String.IsNullOrEmpty(GroupNameFilter))
            {
                foreach (var tmpAdministrativeInformation in AllAdministrativeInformations)
                {
                    CurrentAdministrativeInformationsInComboBox.Add(tmpAdministrativeInformation);
                }
                return;
            }
            foreach (var tmp in AllAdministrativeInformations)
            {
                if((tmp.GroupName).ToLower().Contains(GroupNameFilter.ToLower()))
                {
                    CurrentAdministrativeInformationsInComboBox.Add(tmp);
                }
            }
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