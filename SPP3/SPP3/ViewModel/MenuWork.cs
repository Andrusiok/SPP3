using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.IO;
using SPP3.Model;
using Microsoft.Win32;

namespace SPP3.ViewModel
{
    public sealed class MenuWork : ObservableObject
    {

        private ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();
        public Methods SelectedMethod = null;
        public int SelectedTab{ 
        get { return _selectedTab; }
            set 
            { 
                if (_selectedTab == value) return; 
                _selectedTab = value;
                if (value != -1) SaveActivated = !fwork.savedpaths[value];
                else SaveActivated = false;
                RaisePropertyChangedEvent("SelectedTab");
                RaisePropertyChangedEvent("SelectedFile");
            } 
        }

        public bool SaveActivated
        {
            get { return _saveActive; }
            set {
                if (_saveActive == value) return;
                _saveActive = value;
                RaisePropertyChangedEvent("SaveActivated");
                RaisePropertyChangedEvent("IsEnabled");
            }
        }

        public bool Activated
        {
            get { return _active; }
            set
            {
                if (_active == value) return;
                _active = value;
                RaisePropertyChangedEvent("Activated");
                RaisePropertyChangedEvent("IsEnabled");
            }
        }

        public IEnumerable<TabItem> Tabs { get { return _tabs; } }
        public Filework fwork = new Filework();

        public MenuWork()
        {

        }

        private ICommand _aboutApp;
        private ICommand _closeApp;
        private ICommand _closeFile;
        private ICommand _saveFile;
        private ICommand _saveFileAs;
        private ICommand _openFile;
        private ICommand _exitFromApp;

        private int _selectedTab;
        private bool _exited = false;
        private bool _active = false;
        private bool _saveActive = false;

        public ICommand AboutApp
        {
            get { return _aboutApp ?? (_aboutApp = new DelegateCommand(() => ShowAboutAppBox())); }
        }

        public ICommand OpenFile
        {
            get { return _openFile ?? (_openFile = new DelegateCommand(() => OpeningProcess())); }
        }

        public ICommand CloseFile
        {
            get { return _closeFile ?? (_closeFile = new DelegateCommand(() => ClosingProcess())); }
        }

        public ICommand SaveFile
        {
            get { return _saveFile ?? (_saveFile = new DelegateCommand(() => SavingProcess())); }
        }

        public ICommand SaveFileAs
        {
            get { return _saveFileAs ?? (_saveFileAs = new DelegateCommand(() => SavingAsProcess())); }
        }

        public ICommand CloseApp
        {
            get { return _closeApp ?? (_closeApp = new DelegateCommand(() => CloseApplication())); }
        }

       // public ICommand ExitFromApp
       // {
        //    get { return _exitFromApp ?? (_exitFromApp = new DelegateCommand(() => OnExit())); }
        //}

        private void ShowAboutAppBox()
        {
            MessageBox.Show("Лабораторная работа №3\nПопов Андрей 2016");
        }

        private void ClosingProcess()
        {
            if (_selectedTab != -1)
                if (fwork.savedpaths[_selectedTab])
                {
                    fwork.fullpaths.RemoveAt(_selectedTab);
                    fwork.savedpaths.RemoveAt(_selectedTab);
                    _tabs.RemoveAt(_selectedTab);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(string.Format("Сохранить файл как \"{0}\"?", _tabs[_selectedTab].Header), "", MessageBoxButton.YesNoCancel);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            fwork.SavingXMLFile(fwork.fullpaths[_selectedTab], _tabs[_selectedTab].ThreadsList);
                            fwork.fullpaths.RemoveAt(_selectedTab);
                            fwork.savedpaths.RemoveAt(_selectedTab);
                            _tabs.RemoveAt(_selectedTab);
                            break;
                        case MessageBoxResult.No:
                            fwork.fullpaths.RemoveAt(_selectedTab);
                            fwork.savedpaths.RemoveAt(_selectedTab);
                            _tabs.RemoveAt(_selectedTab);
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }

                }
            if (_selectedTab == -1) Activated = false;
        }

        private void OpeningProcess()
        {
            OpenFileDialog openFileDialog = CreateOpenDialog();
            Stream stream = null;
            XMLTreeAsm Constructor = null;
            string shortpath;
            string fullpath;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if ((stream = openFileDialog.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            shortpath = openFileDialog.SafeFileName;
                            fullpath = openFileDialog.FileName;
                            if (fwork.fullpaths.Contains(fullpath))
                            {
                                SelectedTab = fwork.fullpaths.IndexOf(fullpath);
                            }
                            else
                            { 
                                Constructor = new XMLTreeAsm(stream);
                                CreateTabPage(shortpath, Constructor);
                                fwork.fullpaths.Add(fullpath);
                                fwork.savedpaths.Add(true);
                                Activated = true;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void SavingProcess()
        {
            fwork.SavingXMLFile(fwork.fullpaths[_selectedTab], _tabs[_selectedTab].ThreadsList);
            fwork.savedpaths[_selectedTab] = true;
            SaveActivated = false;
        }

        private void SavingAsProcess()
        {
            SaveFileDialog saveFileDialog = CreateSaveDialog();
            if (_selectedTab != -1)
                if (saveFileDialog.ShowDialog()==true)
                {
                    fwork.SavingXMLFile(saveFileDialog.FileName, _tabs[_selectedTab].ThreadsList);
                    fwork.savedpaths[_selectedTab] = true;
                    fwork.fullpaths[_selectedTab] = saveFileDialog.FileName;
                    _tabs[_selectedTab].Header = saveFileDialog.SafeFileName;
                }
        }

        private void CloseApplication()
        {
            CloseAllTabs();
            _exited = true;
        }

        private void CloseAllTabs()
        {
            int i = 0;

            foreach (TabItem tab in _tabs)
            {
                if (!fwork.savedpaths[i])
                {
                    MessageBoxResult result = MessageBox.Show(string.Format("Сохранить файл как \"{0}\"?", _tabs[i].Header), "", MessageBoxButton.YesNoCancel);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            fwork.SavingXMLFile(fwork.fullpaths[i], _tabs[i].ThreadsList);
                            break;
                        case MessageBoxResult.No:
                            fwork.fullpaths.RemoveAt(i);
                            break;
                    }
                 }
                i++;
            }
        }

        public void OnExit(object sender, EventArgs e)
        {
            if (!_exited)
            {
                CloseAllTabs();
            }
        }

        private OpenFileDialog CreateOpenDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";

            return openFileDialog;
        }

        private SaveFileDialog CreateSaveDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml)|*.xml";

            return saveFileDialog;
        }

        private void CreateTabPage(string header, XMLTreeAsm constructor)
        {
            List<Threads> threadList = null;
            threadList = constructor.FillList();
            TabItem tab = new TabItem(header);
            tab.InitializeThreadsList(threadList);
            _tabs.Add(tab);
        }

    }

    public sealed class TabItem : ObservableObject
    {
        private ObservableCollection<Threads> _threadsList = new ObservableCollection<Threads> { };
        public IEnumerable<Threads> ThreadsList { get { return _threadsList; } }

        private string _header;
        public string Header { get { return _header; } set {
                if (_header == value) return;
                _header = value;
                RaisePropertyChangedEvent("Header");
            } }

        public TabItem(string header)
        {
            _header = header;
        }

        public void InitializeThreadsList(List<Threads> list)
        {
            foreach(Threads item in list)
            {
                _threadsList.Add(item);
            }
        }
    }

    public sealed class Threads : ObservableObject
    {
        private ObservableCollection<Methods> _methodsList = new ObservableCollection<Methods> { };
        public IEnumerable<Methods> MethodsList { get { return _methodsList; } }
        public object Parent = null;

        private int _threadID;
        private int _time;

        public int ThreadID { get { return _threadID; }
            set
            {
                if (_threadID == value) return;
                _threadID = value;
                RaisePropertyChangedEvent("ThreadID");
            }
        }

        public override int Time
        {
            get { return _time; }
            set
            {
                if (_time == value) return;
                _time = value;
                RaisePropertyChangedEvent("Time");
            }
        }


        public Threads(int threadID, int time)
        {
            _threadID = threadID;
            _time = time;
        }

        override public void Add(Methods method)
        {
            _methodsList.Add(method);
        }
    }

    public sealed class Methods : ObservableObject
    {
        private ObservableCollection<Methods> _methodsList = new ObservableCollection<Methods> { };
        public IEnumerable<Methods> MethodsList { get { return _methodsList; } }
        //public object Parent;

        private int _time;
        private int _paramsCount;
        private string _package;
        private string _name;
        private bool _clicked = false;

        public bool Clicked
        {
            get { return _clicked;  }
            set { _clicked = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChangedEvent("Name");
            }
        }

        public string Package
        {
            get { return _package; }
            set
            {
                if (_package == value) return;
                _package = value;
                RaisePropertyChangedEvent("Package");
            }
        }

        public int ParamsCount
        {
            get { return _paramsCount; }
            set
            {
                if (_paramsCount == value) return;
                _paramsCount = value;
                RaisePropertyChangedEvent("ParamsCount");
            }
        }

        public override int Time
        {
            get { return _time; }
            set
            {
                if (_time == value) return;
                _time = value;
                RaisePropertyChangedEvent("Time");
            }
        }

        public int Count
        {
            get { return _methodsList.Count; }
        }

        public Methods(int time, int paramsCount, string package, string name, ObservableObject parent)
        {
            _time = time;
            _paramsCount = paramsCount;
            _package = package;
            _name = name;

            Parent = parent;
        }

        override public void Add(Methods method)
        {
            _methodsList.Add(method);
        }

        public void OnItemMouseDoubleClick(Props data)
        {
            Name = data.name;
            Package = data.package;
            ParamsCount = data.paramsc;

            int difference = XMLReorganization.FindDifference(_time, data.time);
            Time -= difference;

            ChangeTime(Parent as ObservableObject, difference);

        }

        private void ChangeTime(ObservableObject parent, int difference)
        {
            parent.Time -= difference;

            if (parent.Parent != null) ChangeTime(parent.Parent as ObservableObject, difference);
            
        }
    }

}

