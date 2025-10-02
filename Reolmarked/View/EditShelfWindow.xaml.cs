using Reolmarked.Model;
using Reolmarked.ViewModel;
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

namespace Reolmarked.View
{
    /// <summary>
    /// Interaction logic for EditShelfWindow.xaml
    /// </summary>
    public partial class EditShelfWindow : Window
    {
        public EditShelfWindow(Shelf shelf)
        {
            InitializeComponent();
            DataContext = new EditShelfViewModel(shelf);
            this.Loaded += EditShelfWindow_Loaded;
        }

        private void EditShelfWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditShelfViewModel viewModel)
            {
                viewModel.RequestClose += (s, args) => this.Close();
            }
        }
    }
}