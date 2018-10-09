using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace University_System.ViewModel
{
    public class AppViewModel :INotifyPropertyChanged
    {
        private IEnumerable _dataGridInformation;
        // INotifyPropertyChanged -> DependencyProperty?
        public IEnumerable DataGridInformation
        {
            get => _dataGridInformation;
            set
            {
                _dataGridInformation = value;
                OnPropertyChanged();
            }
        }

        public  StudentViewModel StudentViewModel { get; set; }

        public AppViewModel()
        {
            StudentViewModel = new StudentViewModel();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
