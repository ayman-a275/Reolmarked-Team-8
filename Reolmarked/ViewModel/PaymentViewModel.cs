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
        public ICommand AddProductToPaymentBtnClickCommand { get; }
        public ICommand PayBtnClickCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");
        public List<string> PaymentMethod { get; set; }
        public string SelectedPaymentMethod { get; set; }

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
            if(SelectedPaymentMethod == "MobilePay")
            {
                ProductsToPay.Clear();
                ProductsTotalPrice = 0;
            }
            else if(SelectedPaymentMethod == "Kontant")
            {
                ProductsToPay.Clear();
                ProductsTotalPrice = 0;
                PaymentChange = 15;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
