using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using University_System.Commands;
using University_System.Models;
using University_System.ViewModel;

namespace University_System
{
    /// <summary>
    /// Interaction logic for AddNewStudentWindow.xaml
    /// </summary>
    public partial class AddNewStudentWindow : Window
    {
        private StudentViewModel _studentViewModel;
        private int _countOfCharactersInTextBox;

        public AddNewStudentWindow(StudentViewModel studentViewModel)
        {
            InitializeComponent();
            _studentViewModel = studentViewModel;
            DataContext = _studentViewModel;

            _studentViewModel.SaveButtonClickAddNewStudentWindow = new MyCommand(Save_Click);
            _countOfCharactersInTextBox = 0;
        }

        private void Save_Click()
        {
            DialogResult = true;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = (TextBox) sender;
            string number = txtBox.Text, newNumber = "";
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

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).SelectionLength = 0;
            ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).UpdateLayout();
        }
    }
}
