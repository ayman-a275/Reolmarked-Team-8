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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var _menuView = new MenuView();
            var _menuClosedView = new MenuClosedView();
            MenuContent.Content = _menuView;

            _menuView.ProductBtn.Click += (s, e) =>
            {
                MainContent.Content = new ProductView();
                DataContext = new ProductViewModel();
            };

            _menuView.ShelfBtn.Click += (s, e) =>
            {
                MainContent.Content = new ShelfView();
                DataContext = new ShelfViewModel();
            };

            _menuView.PaymentBtn.Click += (s, e) =>
            {
                MainContent.Content = new PaymentView();
                DataContext = new PaymentViewModel();
                MenuContent.Content = _menuClosedView;
            };

            _menuView.RenterBtn.Click += (s, e) =>
            {
                MainContent.Content = new RenterView();
                DataContext = new RenterViewModel();
            };

            _menuView.RentShelfBtn.Click += (s, e) =>
            {
                MainContent.Content = new RentShelfView();
                DataContext = new RentShelfViewModel();
            };

            _menuView.MonthlySettlementBtn.Click += (s, e) =>
            {
                MainContent.Content = new MonthlySettlementView();
                DataContext = new MonthlySettlementViewModel();
            };

            _menuView.CloseMenuBtn.Click += (s, e) =>
            {
                MenuContent.Content = _menuClosedView;
            };

            _menuClosedView.OpenMenuBtn.Click += (s, e) =>
            {
                MenuContent.Content = _menuView;
            };

            _menuView.ExitBtn.Click += (s, e) =>
            {
                Application.Current.Shutdown();
            };
        }
    }
}