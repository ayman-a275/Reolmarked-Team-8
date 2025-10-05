using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Reolmarked.Command;
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
using Reolmarked.Helper;

namespace Reolmarked.ViewModel
{
    public class PaymentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> ProductsToPay { get; set; }
        public ObservableCollection<PaymentMethod> PaymentMethods { get; set; }
        private int? _paymentMethodId;
        private string _productSerialNumber;
        private decimal _productsTotalPrice;
        private decimal? _paymentPaidAmount;
        public ICommand AddProductToPaymentBtnClickCommand { get; }
        public ICommand PayBtnClickCommand { get; }

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

        public decimal? PaymentPaidAmount
        {
            get => _paymentPaidAmount;
            set
            {
                _paymentPaidAmount = value;
                OnPropertyChanged();
            }
        }

        public int? PaymentMethodId
        {
            get => _paymentMethodId;
            set
            {
                _paymentMethodId = value;
                OnPropertyChanged();
            }
        }

        public PaymentViewModel()
        {
            using var context = DbContextFactory.CreateContext();
            ProductsToPay = new ObservableCollection<Product>();
            PaymentMethods = new ObservableCollection<PaymentMethod>(context.PaymentMethod.ToList());
            AddProductToPaymentBtnClickCommand = new RelayCommand(AddProductToPaymentBtnClick);
            PayBtnClickCommand = new RelayCommand(PayBtnClick);
        }

        private void AddProductToPaymentBtnClick()
        {
            if (string.IsNullOrWhiteSpace(ProductSerialNumber))
            {
                MessageBox.Show("Udfyld vare serienummer.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var context = DbContextFactory.CreateContext();

            if (!context.Product.Any(p => p.ProductSerialNumber == ProductSerialNumber))
            {
                MessageBox.Show("Vare serienummer findes ikke.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product ProductToPay = context.Product.FirstOrDefault(p => p.ProductSerialNumber == ProductSerialNumber);
            ProductsToPay.Add(ProductToPay);
            ProductsTotalPrice += ProductToPay.ProductPrice;

            ProductSerialNumber = string.Empty;
        }

        private void PayBtnClick()
        {
            if (ProductsToPay.Count == 0)
            {
                MessageBox.Show("Tilføj min. 1 vare.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PaymentMethodId == null)
            {
                MessageBox.Show("Vælg betalingsmetode.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PaymentPaidAmount == null || PaymentPaidAmount < ProductsTotalPrice)
            {
                MessageBox.Show("Betalingsbeløbet skal være større end prisen.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var context = DbContextFactory.CreateContext();

            var PaymentChange = (PaymentPaidAmount - ProductsTotalPrice);
            var newTransaction = new Transaction(DateTime.Now, ProductsTotalPrice, (decimal)PaymentPaidAmount, (int)PaymentMethodId);
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
            PaymentPaidAmount = null;
            ProductSerialNumber = string.Empty;
            PaymentMethodId = null;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
