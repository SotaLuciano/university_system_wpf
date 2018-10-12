using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace University_System.Models
{
    public class Group: INotifyPropertyChanged
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

        private int _specializationId;

        public int SpecializationId
        {
            get => _specializationId;
            set
            {
                _specializationId = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public  string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
