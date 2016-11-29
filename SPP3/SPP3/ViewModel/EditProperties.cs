using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using SPP3.Model;

namespace SPP3.ViewModel
{
    public sealed class EditProperties : ObservableObject
    {
        private string _name;
        private string _package;
        private int _time;
        private int _paramsCount;
        private bool _isActive = true;
        private Window _pointer;
        private MainWindow parent;

        private ICommand _check;

        public bool Active
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChangedEvent("Active");
            }
        }

        public ICommand Check
        {
            get { return _check ?? (_check = new DelegateCommand(() => OnCheck())); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value;
                RaisePropertyChangedEvent("Name");
            }
        }

        public string Package
        {
            get { return _package; }
            set { _package = value;
                RaisePropertyChangedEvent("Package");
            }
        }

        public int Time
        {
            get { return _time; }
            set { _time = value;
                RaisePropertyChangedEvent("Time");
            }
        }

        public int ParamsCount
        {
            get { return _paramsCount; }
            set { _paramsCount = value;
                RaisePropertyChangedEvent("ParamsCount");
            }
        }

        private void OnCheck()
        {
            parent.props = new Props(_name, _package, _time, _paramsCount);
            _pointer.DialogResult = true;
        }

        public EditProperties(string o1, string o2, int o3, int o4, Window pointer, MainWindow parent)
        {
            _name = o1;
            _package = o2;
            _time = o3;
            _paramsCount = o4;
            _pointer = pointer;
            this.parent = parent;
        }
    }
}
