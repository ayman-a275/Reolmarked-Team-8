using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Reolmarked.Model
{
    public class Product :INotifyPropertyChanged
    {
        private string _productSerialNumber;
        private decimal _productPrice;
        private string? _productDescription;
        private bool _productSold;
        private int _shelfNumber;

        public Product(string productSerialNumber, string productDescription, decimal productPrice, int shelfNumber)
        {
            ProductSerialNumber = productSerialNumber;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductSold = false;
            ShelfNumber = shelfNumber;
        }

        [Key]
        public string ProductSerialNumber {
            get => _productSerialNumber;
            set
            {
                _productSerialNumber = value;
                OnPropertyChanged();
            }
        }

        [AllowNull]
        public string ProductDescription
        {
            get => _productDescription ?? string.Empty;
            set
            {
                _productDescription = value;
                OnPropertyChanged();
            }
        }

        public decimal ProductPrice
        {
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged();
            }
        }

        public bool ProductSold
        {
            get => _productSold;
            set
            {
                _productSold = value;
                OnPropertyChanged();
            }
        }

        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
