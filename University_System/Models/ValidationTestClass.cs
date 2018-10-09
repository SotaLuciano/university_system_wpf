using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace University_System.Models
{
    public class ValidationTestClass : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> propErrors = new Dictionary<string, List<string>>();
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }
        private void Validate()
        {
            Task.Run(() => DataValidation());
        }
        private void DataValidation()
        {
            //Validate Name property
            if (propErrors.TryGetValue(Name, out List<string> listErrors) == false)
                listErrors = new List<string>();
            else
                listErrors.Clear();

            if (string.IsNullOrEmpty(Name))
                listErrors.Add("Name should not be empty!!!");
            else if (string.Equals(Name, "0"))
                listErrors.Add("Enter a different name!!!");
            propErrors["Name"] = listErrors;

            if (listErrors.Count > 0)
            {
                OnPropertyErrorsChanged("Name");
            }
        }
        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnPropertyErrorsChanged(string p)
        {
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(p));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();
            if (propertyName != null)
            {
                propErrors.TryGetValue(propertyName, out errors);
                return errors;
            }

            else
                return null;
        }

        public bool HasErrors
        {
            get
            {
                try
                {
                    var propErrorsCount = propErrors.Values.FirstOrDefault(r => r.Count > 0);
                    if (propErrorsCount != null)
                        return true;
                    else
                        return false;
                }
                catch { }
                return true;
            }
        }

        # endregion
        /// <new>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Validate();
            }
        }
    }
}
