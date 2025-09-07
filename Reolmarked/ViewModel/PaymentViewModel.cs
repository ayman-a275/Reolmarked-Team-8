using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Reolmarked.Command;
using Reolmarked.Data;
using Reolmarked.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Reolmarked.ViewModel
{
    public class PaymentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> ProductsToPay { get; set; }
        private string _productSerialNumber;
        private decimal _productsTotalPrice;
        private decimal _paymentChange;
        private decimal? _paidPrice;
        private bool _chosenPaymentMethod;
        public string _selectedPaymentMethod;
        public ICommand AddProductToPaymentBtnClickCommand { get; }
        public ICommand PayBtnClickCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");
        public List<string> PaymentMethod { get; set; }

        public string ProductSerialNumber
        {
            get => _productSerialNumber;
            set
            {
                _productSerialNumber = value;
                OnPropertyChanged();
            }
        }

        public decimal ProductsTotalPrice
        {
            get => _productsTotalPrice;
            set
            {
                _productsTotalPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal PaymentChange
        {
            get => _paymentChange;
            set
            {
                _paymentChange = value;
                OnPropertyChanged();
            }
        }

        public decimal? PaidPrice
        {
            get => _paidPrice;
            set
            {
                _paidPrice = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set
            {
                _selectedPaymentMethod = value;
                OnPropertyChanged();
                ChangedPaymentMethod();
            }
        }

        public bool ChosenPaymentMethod
        {
            get => _chosenPaymentMethod;
            set
            {
                _chosenPaymentMethod = value;
                OnPropertyChanged();
            }
        }

        public PaymentViewModel()
        {
            using var context = new AppDbContext(connectionString);
            ProductsToPay = new ObservableCollection<Product>();
            AddProductToPaymentBtnClickCommand = new RelayCommand(AddProductToPaymentBtnClick);
            PayBtnClickCommand = new RelayCommand(PayBtnClick);
            PaymentMethod = new List<string>() { "MobilePay", "Kontant" };
        }

        private void AddProductToPaymentBtnClick()
        {
            using var context = new AppDbContext(connectionString);
            Product ProductToPay = context.Product.FirstOrDefault(p => p.ProductSerialNumber == ProductSerialNumber);
            ProductsToPay.Add(ProductToPay);
            ProductsTotalPrice += ProductToPay.ProductPrice;

        }

        private void PayBtnClick()
        {
            using var context = new AppDbContext(connectionString);
            if(SelectedPaymentMethod == "MobilePay")
            {
                //context.Transaction.Add(new Transaction())
                ProductsToPay.Clear();
                ProductsTotalPrice = 0;
            }
            else if(SelectedPaymentMethod == "Kontant")
            {
                ProductsToPay.Clear();
                MessageBox.Show($"Byttepenge: {PaymentChange = (decimal)(PaidPrice - ProductsTotalPrice)}", "Byttepenge", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                ProductsTotalPrice = 0;
                PaidPrice = null;
            }
        }

        private void ChangedPaymentMethod()
        {
            if (SelectedPaymentMethod == "MobilePay")
            {
                PaidPrice = null;
                ChosenPaymentMethod = false;
            }
            else if (SelectedPaymentMethod == "Kontant")
            {
                ChosenPaymentMethod = true;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
