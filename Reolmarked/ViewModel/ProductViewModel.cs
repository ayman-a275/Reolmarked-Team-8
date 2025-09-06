using Microsoft.Extensions.Configuration;
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
    public class ProductViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Rack> Racks { get; set; }
        private string _productSerialNumber;
        private decimal _productPrice;
        private int _rackNumber;
        public ICommand AddProductBtnClickCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");
        public int SelectedRackNumber { get; set; }

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

        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel()
        {
            using var context = new AppDbContext(connectionString);
            Products = new ObservableCollection<Product>(context.Product.ToList());
            Racks = new ObservableCollection<Rack>(context.Rack.ToList());
            AddProductBtnClickCommand = new RelayCommand(AddProductBtnClick);
        }

        private async void AddProductBtnClick()
        {
            var newProduct = new Product(ProductSerialNumber, ProductPrice, SelectedRackNumber);
            Products.Add(newProduct);

            try
            {
                await Task.Run(() =>
                {
                    var dbProduct = new Product(ProductSerialNumber, ProductPrice, SelectedRackNumber);

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException("Database connection string is null or empty.");
                    }

                    using (var db = new AppDbContext(connectionString))
                    {
                        db.Product.Add(dbProduct);
                        db.SaveChanges();
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding product to database: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
