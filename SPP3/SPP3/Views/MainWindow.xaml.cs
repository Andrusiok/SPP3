using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using SPP3.ViewModel;
using SPP3.Views;
using System.Xaml;

namespace SPP3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuWork pointer;
        public Props props = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MenuWork();
            pointer = DataContext as MenuWork;
        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBlock txt = sender as TextBlock;
            object o = txt.DataContext;
            Methods met = o as Methods;

            if (object.ReferenceEquals(pointer.SelectedMethod, met))
            {
                Edit edit = new Edit(met.Name, met.Package, met.Time, met.ParamsCount, this);
                bool? result = edit.ShowDialog();

                if (result ?? true)
                {
                    met.OnItemMouseDoubleClick(props);
                    pointer.SelectedMethod = null;
                    pointer.fwork.savedpaths[pointer.SelectedTab] = false;
                    pointer.SaveActivated = true;
                }

                
            }              
            else pointer.SelectedMethod = met;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            pointer.OnExit(sender, e);
        }
    }
}
