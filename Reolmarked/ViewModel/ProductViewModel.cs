using Microsoft.Extensions.Configuration;
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
using System.Windows.Interop;
using Reolmarked.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Reolmarked.View;

namespace Reolmarked.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Rack> Racks { get; set; }
        private Rack? _selectedRack { get; set; }
        private int _rackNumber;
        private string _productSerialNumber;
        private string? _productDescription;
        private decimal? _productPrice;
        public ICommand AddProductBtnClickCommand { get; }
        public ICommand GenerateBarCodeBtnClickCommand { get; }
        public ICommand PrintBarCodeBtnClickCommand { get; }
        public ICommand EditProductCommand { get; }

        public Rack SelectedRack
        {
            get => _selectedRack;
            set
            {
                _selectedRack = value;
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

        public string ProductSerialNumber
        {
            get => _productSerialNumber;
            set
            {
                _productSerialNumber = value;
                OnPropertyChanged();
            }
        }

        public string? ProductDescription
        {
            get => _productDescription ?? string.Empty;
            set
            {
                _productDescription = value;
                OnPropertyChanged();
            }
        }

        public decimal? ProductPrice
        {
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel()
        {
            using var context = DbContextFactory.CreateContext();
            Products = new ObservableCollection<Product>(context.Product.Where(p => p.ProductSold == false).ToList());
            Racks = new ObservableCollection<Rack>(context.Rack.ToList());

            AddProductBtnClickCommand = new RelayCommand(AddProductBtnClick);
            GenerateBarCodeBtnClickCommand = new RelayCommand(GenerateBarCodeBtnClick);
            PrintBarCodeBtnClickCommand = new RelayCommand(PrintBarCodeBtnClick);
            EditProductCommand = new RelayCommand(EditProduct);
        }

        private async void AddProductBtnClick()
        {
            if (SelectedRack == null)
            {
                System.Windows.MessageBox.Show("Reol nummer skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (string.IsNullOrWhiteSpace(ProductSerialNumber))
            {
                System.Windows.MessageBox.Show("Serie nummer skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (!ProductPrice.HasValue || ProductPrice <= 0)
            {
                System.Windows.MessageBox.Show("Pris skal være større end 0.", "Valideringsfejl");
                return;
            }

            var newProduct = new Product(ProductSerialNumber, ProductDescription!, ProductPrice!.Value, SelectedRack.RackNumber);

            try
            {
                await Task.Run(() =>
                {
                    using var context = DbContextFactory.CreateContext();
                    context.Product.Add(newProduct);
                    context.SaveChanges();
                });

                Products.Add(newProduct);

                SelectedRack = null;
                ProductSerialNumber = string.Empty;
                ProductDescription = string.Empty;
                ProductPrice = null;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding product to database: {ex.Message}");
                System.Windows.MessageBox.Show($"Fejl ved tilføjelse af produkt: {ex.Message}", "Fejl",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void GenerateBarCodeBtnClick()
        {
            ProductSerialNumber = SerialNumberGenerator.GenerateRandomString();
        }

        private void PrintBarCodeBtnClick()
        {
            if (string.IsNullOrWhiteSpace(ProductSerialNumber))
            {
                System.Windows.MessageBox.Show("Serie nummer skal udfyldes.", "Valideringsfejl");
                return;
            }

            PrintBarCode.PrintBarcode(ProductSerialNumber);
        }

        private void EditProduct(object? parameter)
        {
            if (parameter is Product product)
            {
                var editProductWindow = new EditProductWindow(product);
                editProductWindow.ShowDialog();
                OnPropertyChanged();
                RefreshProductsList();
            }
        }

        private void RefreshProductsList()
        {
            try
            {
                using var context = DbContextFactory.CreateContext();
                Products.Clear();
                var updatedProducts = context.Product.Where(p => p.ProductSold == false).ToList();
                foreach (var product in updatedProducts)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error refreshing products list: {ex.Message}");
                System.Windows.MessageBox.Show($"Fejl ved opdatering af produktliste: {ex.Message}", "Fejl",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
