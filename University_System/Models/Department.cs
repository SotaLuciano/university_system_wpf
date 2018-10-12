using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace University_System.Models
{
    public class Department :INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _instituteId;

        public int InstituteId
        {
            get => _instituteId;
            set
            {
                _instituteId = value;
                OnPropertyChanged();
            }
        }

        private string _location;

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        public  virtual Institute Institute { get; set; }
        public  virtual ICollection<Specialization> Specializations { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
