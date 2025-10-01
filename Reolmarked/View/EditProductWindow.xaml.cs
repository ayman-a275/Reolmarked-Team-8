using Reolmarked.Model;
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
using Reolmarked.ViewModel;

namespace Reolmarked.View
{
    /// <summary>
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public EditProductWindow(Product product)
        {
            InitializeComponent();
            DataContext = new EditProductViewModel(product);
            this.Loaded += EditProductWindow_Loaded;
        }

        private void EditProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditProductViewModel viewModel)
            {
                viewModel.RequestClose += (s, args) => this.Close();
            }
        }
    }
}
