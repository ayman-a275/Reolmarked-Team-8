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
        public ObservableCollection<PaymentMethod> PaymentMethods { get; set; }
        public int PaymentMethodId { get; set; }
        private string _productSerialNumber;
        private decimal _productsTotalPrice;
        private decimal _paymentPaidAmount;
        public ICommand AddProductToPaymentBtnClickCommand { get; }
        public ICommand PayBtnClickCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

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

        public decimal PaymentPaidAmount
        {
            get => _paymentPaidAmount;
            set
            {
                _paymentPaidAmount = value;
                OnPropertyChanged();
            }
        }

        public PaymentViewModel()
        {
            using var context = new AppDbContext(connectionString);
            ProductsToPay = new ObservableCollection<Product>();
            PaymentMethods = new ObservableCollection<PaymentMethod>(context.PaymentMethod.ToList());
            AddProductToPaymentBtnClickCommand = new RelayCommand(AddProductToPaymentBtnClick);
            PayBtnClickCommand = new RelayCommand(PayBtnClick);
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

            var PaymentChange = (PaymentPaidAmount - ProductsTotalPrice);
            var newTransaction = new Transaction(DateTime.Now, ProductsTotalPrice, PaymentPaidAmount, PaymentMethodId);
            context.Transaction.Add(newTransaction);
            context.SaveChanges();
            foreach (Product newProduct in ProductsToPay)
            {
                context.TransactionLine.Add(new TransactionLine(newTransaction.TransactionId, newProduct.ProductSerialNumber));
                var productToChange = context.Product.FirstOrDefault(p => p.ProductSerialNumber == newProduct.ProductSerialNumber);
                productToChange.ProductSold = true;
            }
            context.SaveChanges();
            ProductsToPay.Clear();
            MessageBox.Show($"Byttepenge: {PaymentChange}", "Byttepenge", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            ProductsTotalPrice = 0;
            PaymentPaidAmount = 0;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
