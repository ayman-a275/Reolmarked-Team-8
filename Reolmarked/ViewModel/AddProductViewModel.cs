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
using System.Windows.Input;

namespace Reolmarked.ViewModel
{
    public class AddProductViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        private string _productSerialNumber;
        private decimal _productPrice;
        public ICommand AddProductBtnClickCommand { get; }

        public string ProductSerialNumber
        {
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

        public AddProductViewModel()
        {
            Products = new() {
                new Product("dskdfj22", 22)
            };
            AddProductBtnClickCommand = new RelayCommand(AddProductBtnClick);
        }

        private void AddProductBtnClick()
        {
            Products.Add(new Product(ProductSerialNumber, ProductPrice));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
