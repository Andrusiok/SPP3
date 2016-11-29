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
using System.Windows.Shapes;
using SPP3.ViewModel;

namespace SPP3.Views
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        EditProperties pointer = null;
        
        public Edit(string o1, string o2, int o3, int o4, MainWindow owner)
        {
            InitializeComponent();
            DataContext = new EditProperties(o1, o2, o3, o4, this, owner);
            pointer = DataContext as EditProperties;
            
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
            pointer.Active = !binding.HasError;
            
        }
    }
}
