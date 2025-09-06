using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Reolmarked.Model
{
    public class Product :INotifyPropertyChanged
    {
        private string _productSerialNumber;
        private decimal _productPrice;
        private int _rackNumber;

        public Product(string productSerialNumber, decimal productPrice, int rackNumber)
        {
            _productSerialNumber = productSerialNumber;
            _productPrice = productPrice;
            _rackNumber = rackNumber;
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

        public decimal ProductPrice
        {
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged();
            }
        }

        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
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
