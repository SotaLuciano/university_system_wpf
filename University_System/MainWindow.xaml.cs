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

        private void InfoGrid_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            var grid = (DataGrid) sender;
            var currentCell = grid.CurrentCell;
            if (currentCell.Column.Header.ToString() == "PhoneNumber")
            {
                
            }
        }

        private void InfoGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.OriginalSource is TextBox textBox)
            {
                var columnHeader = ((DataGridCell) textBox.Parent).Column.Header.ToString();
                if (columnHeader == "PhoneNumber")
                {
                    textBox.SelectionStart = textBox.Text.Length;

                    textBox.MaxLength = 16;
                    if (textBox.Text.Length == 3)
                    {
                        textBox.Text += "0";
                        textBox.SelectionStart = textBox.Text.Length;
                        return;
                    }

                    string number = textBox.Text, newNumber = "";
                    if (number[number.Length - 1] == ' ')
                    {
                        number = number.Substring(0, number.Length - 1);
                        textBox.Text = number;
                    }
                    if (_countOfCharactersInTextBox >= textBox.Text.Length)
                    {
                        _countOfCharactersInTextBox = textBox.Text.Length;
                        if (number == "+38")
                        {
                            textBox.Text += "0";
                        }
                        return;
                    }

                    if (textBox.Text.Length == 7)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            newNumber += number[i];
                        }
                        newNumber += "-";
                        newNumber += number[6];
                        textBox.Text = newNumber;
                    }
                    else if (textBox.Text.Length == 11)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            newNumber += number[i];
                        }
                        newNumber += "-";
                        newNumber += number[10];
                        textBox.Text = newNumber;
                    }
                    else if (textBox.Text.Length == 14)
                    {
                        for (int i = 0; i < 13; i++)
                        {
                            newNumber += number[i];
                        }
                        newNumber += "-";
                        newNumber += number[13];
                        textBox.Text = newNumber;
                    }

                    textBox.SelectionStart = textBox.Text.Length;
                    _countOfCharactersInTextBox = textBox.Text.Length;

                }
            }
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
    }
}
