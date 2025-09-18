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
    /// Interaction logic for EditRack.xaml
    /// </summary>
    public partial class EditRackWindow : Window
    {
        public EditRackWindow(Rack rack)
        {
            InitializeComponent();
            DataContext = new EditRackViewModel(rack);
            this.Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditRackViewModel viewModel)
            {
                viewModel.RequestClose += (s, args) => this.Close();
            }
        }
    }
}
