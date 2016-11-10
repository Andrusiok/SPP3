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

        public ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();
        public IEnumerable<TabItem> Tabs { get { return _tabs; } }

        public MenuWork()
        {
            _tabs.Add(new TabItem() { Header = "Hey"});
            _tabs.Add(new TabItem() { Header = "Hi" });
        }

        private ICommand _aboutApp;
        private ICommand _closeApp;
        private ICommand _closeFile;
        private ICommand _saveFile;
        private ICommand _saveFileAs;
        private ICommand _openFile;

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

    public sealed class TabItem
    {
        public string Header;
    }
}

