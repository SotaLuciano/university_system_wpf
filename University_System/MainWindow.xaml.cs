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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && DataContext is StudentViewModel studentViewModel)
            {
                if (studentViewModel.IsDatePickerEnable)
                {
                    // Turn off datePicker.
                    studentViewModel.IsDatePickerEnable = false;
                    studentViewModel.IsDatePickerEnableText = "Enable";
                    studentViewModel.IsToDateFilterEnable = false;
                    studentViewModel.IsFromDateFilterEnable = false;
                    studentViewModel.FromDateFilter = new DateTime();
                    studentViewModel.ToDateFilter = new DateTime();
                }
                else
                {
                    // Turn on datePicker.
                    studentViewModel.IsDatePickerEnable = true;
                    studentViewModel.IsDatePickerEnableText = "Disable";
                }
            }
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox 
                && DataContext is StudentViewModel studentViewModel
                && checkBox.DataContext is AdministrativeInformation selectedAdministrativeInformation)
            {
                // Add group to datagrid if it is selected.
                var checkedAdministrativeInformations = studentViewModel.CurrentAdministrativeInformationsInComboBox.FirstOrDefault(x =>
                    x.GroupId == selectedAdministrativeInformation.GroupId);
                // Need to check if there is already this group in grid.
                var alreadyInList = studentViewModel.CurrentAdministrativeInformationsInDataGrid.FirstOrDefault(x =>
                    x.GroupId == selectedAdministrativeInformation.GroupId);

                if (checkedAdministrativeInformations != null)
                {
                    if (checkBox.IsChecked == true && alreadyInList == null)
                    {
                        // Add group to grid.
                        studentViewModel.CurrentAdministrativeInformationsInDataGrid.Add(selectedAdministrativeInformation);
                        studentViewModel.SelectedGroupsNames += selectedAdministrativeInformation.GroupName + " ";
                        selectedAdministrativeInformation.IsSelected = true;
                        // Add stackpanel with groupname and button 'remove'.
                        AddNewGroupToStackPanel(selectedAdministrativeInformation.GroupId, selectedAdministrativeInformation.GroupName);
                    }
                    else if (checkBox.IsChecked == false)
                    {
                        // Remove group.
                        studentViewModel.CurrentAdministrativeInformationsInDataGrid.Remove(selectedAdministrativeInformation);
                        studentViewModel.SelectedGroupsNames = studentViewModel.SelectedGroupsNames.Replace(selectedAdministrativeInformation.GroupName + " ", "");
                        selectedAdministrativeInformation.IsSelected = false;

                        RemoveGroupFromStackPanel(selectedAdministrativeInformation.GroupId);
                    }
                }
            }
        }

        private void AddNewGroupToStackPanel(int groupId, string groupName)
        {
            // Create new item.
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Name = "ID" + groupId.ToString()
            };
            var label = new Label
            {
                Content = groupName
            };
            var button = new Button
            {
                Content = "X",
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.Red
            };
            button.Click += removeGroupButtonClick;
            stackPanel.Children.Add(label);
            stackPanel.Children.Add(button);
            // Add new item to the layout.
            selectedGroupsStackPanel.Children.Add(stackPanel);
        }

        private void RemoveGroupFromStackPanel(int groupId)
        {
            // Find stackpanel with groupname and 'remove' button to remove it;
            var result = LogicalTreeHelper.FindLogicalNode(selectedGroupsStackPanel, "ID" + groupId.ToString());
            if(result != null && result is UIElement stackPanel)
                selectedGroupsStackPanel.Children.Remove(stackPanel);
        }

        private void removeGroupButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button 
                && button.Parent is StackPanel stackPanel 
                && DataContext is StudentViewModel studentViewModel)
            {
                // Deselect group.
                var removedItem = studentViewModel.AllAdministrativeInformations.FirstOrDefault(x =>
                    x.GroupId.ToString() == stackPanel.Name.Replace("ID", ""));
                removedItem.IsSelected = false;
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
        private void TxtBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TxtBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ToggleDrop.IsChecked = false;
            ToggleDrop.IsChecked = true;
        }

        private void TxtBox_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleDrop.IsChecked = true;
        }
    }
}
