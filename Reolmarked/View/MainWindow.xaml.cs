using Reolmarked.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reolmarked.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MenuView.ProductBtn.Click += (s, e) =>
            {
                MainContent.Content = new ProductView();
                DataContext = new ProductViewModel();
            };

            MenuView.RackBtn.Click += (s, e) =>
            {
                MainContent.Content = new RackView();
                DataContext = new RackViewModel();
            };

            MenuView.PaymentBtn.Click += (s, e) =>
            {
                MainContent.Content = new PaymentView();
                DataContext = new PaymentViewModel();
            };
        }
    }
}