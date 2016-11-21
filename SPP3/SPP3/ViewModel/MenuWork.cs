using System;
using System.Collections.Generic;
//using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPP3.Model;
using Microsoft.Win32;

namespace SPP3.ViewModel
{
    public sealed class MenuWork : ObservableObject
    {

        private ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();
        public int SelectedTab{ 
        get { return _selectedTab; }
        set 
        { 
            if (_selectedTab == value) return; 
            _selectedTab = value; 
            RaisePropertyChangedEvent("SelectedTab");
            RaisePropertyChangedEvent("SelectedFile");
        } 
        }

        public IEnumerable<TabItem> Tabs { get { return _tabs; } }

        public MenuWork()
        {
            _tabs.Add(new TabItem("Hey"));
            _tabs.Add(new TabItem("Hi"));
            _tabs.Add(new TabItem("Zapraszamy"));
        }

        private ICommand _aboutApp;
        private ICommand _closeApp;
        private ICommand _closeFile;
        private ICommand _saveFile;
        private ICommand _saveFileAs;
        private ICommand _openFile;
        private int _selectedTab;

        public ICommand AboutApp
        {
            get { return _aboutApp ?? (_aboutApp = new DelegateCommand(() => ShowAboutAppBox())); }
        }

        public ICommand OpenFile
        {
            get { return _openFile ?? (_openFile = new DelegateCommand(() => OpeningProcess())); }
        }

        private void ShowAboutAppBox()
        {
            MessageBox.Show("Лабораторная работа №3\nПопов Андрей 2016");
        }

        private void OpeningProcess()
        {
            OpenFileDialog openFileDialog = CreateOpenDialog();
            Stream stream = null;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if ((stream = openFileDialog.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            ;
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private OpenFileDialog CreateOpenDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml files (*.xml)|*.xml";

            return openFileDialog;
        }

    }

    public sealed class TabItem : ObservableObject
    {
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
    }

    public sealed class Threads : ObservableObject
    {
        private ObservableCollection<Methods> _methodsList = new ObservableCollection<Methods> { };
        public IEnumerable<Methods> MethodsList { get { return _methodsList; } }

        private int _threadID;
        private int _time;

        private string _entity;
        public string Entity { get { return _entity; } 
        }

        public Threads()
        {
            ;
        }
    }

    public sealed class Methods : ObservableObject
    {
        private ObservableCollection<Methods> _methodsList = new ObservableCollection<Methods> { };
        public IEnumerable<Methods> MethodsList { get { return _methodsList; } }
        public Methods()
        {
            ;
        }
    }
}

