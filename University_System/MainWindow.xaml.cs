using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using University_System.Models;
using University_System.ViewModel;
using Xceed.Wpf.AvalonDock.Controls;

namespace University_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Phone number validation check.
        private int _countOfCharactersInTextBox;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StudentViewModel();
            _countOfCharactersInTextBox = 0;
        }

        private void DG_ScrollViewer_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private DataGridRow GetRow(DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        private DataGridCell GetCell(DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        private T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        private DataGridCell GetCurrentCell(DataGrid grid, int row, int column)
        {
            DataGridRow rowContainer = GetRow(grid, row);
            return GetCell(grid, rowContainer, column);
        }

        private void InfoGrid_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                DataGridColumn dataGridColumn = dataGrid.CurrentColumn;

                if (dataGridColumn != null && (dataGridColumn.Header.ToString() == "PhoneNumber" || dataGridColumn.Header.ToString() == "Age"))
                {
                    // Prevent to input character.
                    if (!Char.IsDigit(e.Text, 0))
                        e.Handled = true;
                }
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Phone number validation input.
            if (sender is TextBox txtBox)
            {
                if (txtBox.Text == "")
                    return;

                string number = txtBox.Text;
                string newNumber = "";
                if (number[number.Length - 1] == ' ')
                {
                    number = number.Substring(0, number.Length - 1);
                    txtBox.Text = number;
                }
                if (_countOfCharactersInTextBox > txtBox.Text.Length)
                {
                    _countOfCharactersInTextBox = txtBox.Text.Length;
                    if (number == "+38")
                    {
                        txtBox.Text += "0";
                    }
                    return;
                }

                if (txtBox.Text.Length == 7)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        newNumber += number[i];
                    }
                    newNumber += "-";
                    newNumber += number[6];
                    txtBox.Text = newNumber;
                }
                else if (txtBox.Text.Length == 11)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        newNumber += number[i];
                    }
                    newNumber += "-";
                    newNumber += number[10];
                    txtBox.Text = newNumber;
                }
                else if (txtBox.Text.Length == 14)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        newNumber += number[i];
                    }
                    newNumber += "-";
                    newNumber += number[13];
                    txtBox.Text = newNumber;
                }
                txtBox.SelectionStart = txtBox.Text.Length;
                _countOfCharactersInTextBox = txtBox.Text.Length;
            }
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Set pointer to the end.
            if (sender is TextBox textBox)
            {
                textBox.SelectionLength = 0;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void EventSetterMouseDoubleClick_OnHandler(object sender, MouseEventArgs e)
        {
            if (!(StudentGrid.IsReadOnly || InfoGrid.IsReadOnly))
            {
                return;
            }

            if (sender is DataGridRow dataGridRow && DataContext is StudentViewModel studentViewModel)
            {
                string info = "";

                if (dataGridRow.DataContext is AdministrativeInformation admInfo)
                {
                    // Creating popup text information.
                    info += "GroupID =" + admInfo.GroupId + "\n";
                    info += "GroupName = " + admInfo.GroupName + "\n";
                    info += "SpecializationName = " + admInfo.SpecializationName + "\n";
                    info += "DepartmentName = " + admInfo.DepartmentName + "\n";
                    info += "InstituteName =" + admInfo.InstituteName;
                }
                else if (dataGridRow.DataContext is Student student)
                {
                    // Creating popup text information.
                    info += "ID =" + student.Id + "\n";
                    info += "GroupID = " + student.GroupId + "\n";
                    info += "FirstName = " + student.FirstName + "\n";
                    info += "LastName = " + student.LastName + "\n";
                    info += "Age = " + student.Age + "\n";
                    info += "Gender = " + student.Gender + "\n";
                    info += "Email = " + student.Email + "\n";
                    info += "PhoneNumber = " + student.PhoneNumber + "\n";
                    info += "Address = " + student.Address + "\n";
                    info += "BornDateTime = " + student.BornDateTime;
                }

                // Open popup.
                studentViewModel.IsPopupOpen = true;
                studentViewModel.PopupPlacementMode = PlacementMode.MousePoint;
                studentViewModel.PopupText = info;
            }
        }

        private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
        {
            // Close popup.
            if (DataContext is StudentViewModel studentViewModel)
            {
                studentViewModel.IsPopupOpen = false;
            }
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && DataContext is StudentViewModel studentViewModel)
            {
                if (studentViewModel.IsDatePickerEnable)
                {
                    // Turn off datePicker.
                    studentViewModel.IsDatePickerEnable = false;
                    studentViewModel.IsDatePickerEnableText = "Enable";
                    studentViewModel.FromDateFilter = new DateTime();
                    studentViewModel.ToDateFilter = new DateTime();
                    studentViewModel.IsToDateFilterEnable = false;
                    studentViewModel.IsFromDateFilterEnable = false;
                }
                else
                {
                    // Turn on datePicker.
                    studentViewModel.IsDatePickerEnable = true;
                    studentViewModel.IsDatePickerEnableText = "Disable";
                }
            }
        }

        private string getGender(int index)
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

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox 
                && DataContext is StudentViewModel studentViewModel
                && checkBox.DataContext is AdministrativeInformation selectedAdministrativeInformation)
            {
                // Add group to datagrid if it is selected.
                var checkedAdministrativeInformations = studentViewModel.CurrentAdministrativeInformations.FirstOrDefault(x =>
                    x.GroupId == selectedAdministrativeInformation.GroupId);
                if (checkedAdministrativeInformations == null)
                {
                    studentViewModel.CurrentAdministrativeInformations.Add(selectedAdministrativeInformation);
                }
            }
        }

        private void CheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox
                && DataContext is StudentViewModel studentViewModel
                && checkBox.DataContext is AdministrativeInformation selectedAdministrativeInformation)
            {
                // Remove group from datagrid if it is deselected.
                var  checkedAdministrativeInformations = studentViewModel.CurrentAdministrativeInformations.FirstOrDefault(x =>
                    x.GroupId == selectedAdministrativeInformation.GroupId);
                if (checkedAdministrativeInformations != null)
                {
                    studentViewModel.CurrentAdministrativeInformations.Remove(selectedAdministrativeInformation);
                }
            }
        }

        private void FilterHandler(object sender, EventArgs e)
        {
            // One handler for all filters.
            if (DataContext is StudentViewModel studentViewModel)
            {
                IEnumerable<Student> result = null;
                if (sender is TextBox textBox)
                {
                    result = UseFilter(textBox.Text, getGender(studentViewModel.GenderFilter), studentViewModel.IsFromDateFilterEnable,
                        studentViewModel.FromDateFilter, studentViewModel.IsToDateFilterEnable, studentViewModel.ToDateFilter, studentViewModel.Students);
                }
                else if (sender is ComboBox comboBox && comboBox.SelectedValue != null)
                {
                    result = UseFilter(studentViewModel.LastNameFilter, getGender(comboBox.SelectedIndex), false,
                        studentViewModel.FromDateFilter, false, studentViewModel.ToDateFilter, studentViewModel.Students);
                }
                else if (sender is DatePicker datePicker)
                {
                    result = UseFilter(studentViewModel.LastNameFilter, getGender(studentViewModel.GenderFilter), studentViewModel.IsFromDateFilterEnable,
                        studentViewModel.FromDateFilter, studentViewModel.IsToDateFilterEnable, studentViewModel.ToDateFilter, studentViewModel.Students);
                }

                // Write info to ObservableCollection.
                ObservableCollection<Student> observableResult = new ObservableCollection<Student>();
                foreach (var r in result)
                {
                    observableResult.Add(r);
                }
                studentViewModel.DataGridInformation = observableResult;
            }
        }

        private void InfoGrid_OnInitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.Items.Count > 1)
            {
                if (dataGrid.Items.CurrentItem is Student student)
                {
                    // Setting initial information - 'groupId' and 'phoneNumber'.
                    student.GroupId = (dataGrid.Items[0] as Student).GroupId;
                    student.PhoneNumber = "+380";
                }
            }
        }

        private void MyComboBox_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock
                && textBlock.TemplatedParent is ComboBoxItem comboBoxItem)
            {
                // Get checkbox.
                var children = VisualTreeHelper.GetChild(comboBoxItem, 0);
                if (children != null 
                    && children is Grid grid 
                    && grid.Children[0] is CheckBox checkBox)
                {
                    if (checkBox.IsChecked == false)
                    {
                        checkBox.IsChecked = true;
                    }
                    else
                    {
                        checkBox.IsChecked = false;
                    }

                }
            }
            
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            myComboBox.IsDropDownOpen = true;
        }

        private void MyComboBox_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            myComboBox.IsDropDownOpen = true;
        }

        private void MyComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var content = sender as ComboBox;
            var test = VisualTreeHelper.GetChild(content, 0);
            if (test is Grid dGrid && dGrid.Children[0] is Popup popup)
                popup.Focus();
        }
    }
}
