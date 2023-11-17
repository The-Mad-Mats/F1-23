using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1
{
    public class SelectDriversViewModel : ViewModelBase
    {
        private ObservableCollection<DriverGraph> _allDrivers { get; set; }
        public ObservableCollection<DriverGraph> AllDrivers
        {
            get { return _allDrivers; }
            set
            {
                _allDrivers = value;
                NotifyPropertyChanged("AllDrivers");
            }
        }

    }

    public class DriverGraph
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
    }

}
