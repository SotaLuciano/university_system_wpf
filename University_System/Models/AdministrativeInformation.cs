using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace University_System.Models
{
    public class AdministrativeInformation : INotifyPropertyChanged
    {
        private int _groupId;
        public int GroupId
        {
            get => _groupId;
            set
            {
                _groupId = value;
                OnPropertyChanged();
            }
        }

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                OnPropertyChanged();
            }
        }

        private string _specializationName;
        public string SpecializationName
        {
            get => _specializationName;
            set
            {
                _specializationName = value;
                OnPropertyChanged();
            }
        }

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                _departmentName = value;
                OnPropertyChanged();
            }
        }
        private string _instituteName;
        public string InstituteName
        {
            get => _instituteName;
            set
            {
                _instituteName = value;
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
