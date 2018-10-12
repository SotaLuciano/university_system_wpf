using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using University_System.Models;
using University_System.ViewModel;

namespace University_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                // May be virtualized, bring into view and try again.
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
                    if (!Char.IsDigit(e.Text, 0))
                        e.Handled = true;
                }
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            string number = txtBox.Text, newNumber = "";
            if (number == "")
                return;
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

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).SelectionLength = 0;
            ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
