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
using System.Windows;

namespace Reolmarked.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> _products;
        public ObservableCollection<Shelf> _shelfs;
        private Shelf? _selectedShelf { get; set; }
        private int _shelfNumber;
        private string _productSerialNumber;
        private string? _productDescription;
        private decimal? _productPrice;
        public ICommand AddProductBtnClickCommand { get; }
        public ICommand GenerateBarCodeBtnClickCommand { get; }
        public ICommand PrintBarCodeBtnClickCommand { get; }
        public ICommand EditProductCommand { get; }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Shelf> Shelfs
        {
            get => _shelfs;
            set
            {
                _shelfs = value;
                OnPropertyChanged();
            }
        }

        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                _selectedShelf = value;
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
            Products = new ObservableCollection<Product>();
            Shelfs = new ObservableCollection<Shelf>();
            LoadProductsAsync();
            LoadShelfsAsync();

            AddProductBtnClickCommand = new RelayCommand(AddProductBtnClick);
            GenerateBarCodeBtnClickCommand = new RelayCommand(GenerateBarCodeBtnClick);
            PrintBarCodeBtnClickCommand = new RelayCommand(PrintBarCodeBtnClick);
            EditProductCommand = new RelayCommand(EditProduct);
        }

        private async void LoadProductsAsync()
        {
            try
            {
                var products = await Task.Run(() =>
                {
                    using var context = DbContextFactory.CreateContext();
                    var products = context.Product.Where(p => p.ProductSold == false).ToList();
                    return products;
                });

                Products = new ObservableCollection<Product>(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved forbindelse til database: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadShelfsAsync()
        {
            try
            {
                var shelfs = await Task.Run(() =>
                {
                    using var context = DbContextFactory.CreateContext();
                    var shelfs = context.Shelf.ToList();
                    return shelfs;
                });

                Shelfs = new ObservableCollection<Shelf>(shelfs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved forbindelse til database: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddProductBtnClick()
        {
            if (SelectedShelf == null)
            {
                MessageBox.Show("Reol nummer skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (string.IsNullOrWhiteSpace(ProductSerialNumber))
            {
                MessageBox.Show("Serie nummer skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (!ProductPrice.HasValue || ProductPrice <= 0)
            {
                MessageBox.Show("Pris skal være større end 0.", "Valideringsfejl");
                return;
            }

            var newProduct = new Product(ProductSerialNumber, ProductDescription!, ProductPrice!.Value, SelectedShelf.ShelfNumber);

            try
            {
                await Task.Run(() =>
                {
                    using var context = DbContextFactory.CreateContext();
                    context.Product.Add(newProduct);
                    context.SaveChanges();
                });

                Products.Add(newProduct);

                MessageBox.Show("Vare gemt succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                SelectedShelf = null;
                ProductSerialNumber = string.Empty;
                ProductDescription = string.Empty;
                ProductPrice = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved tilføjelse af produkt: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Serie nummer skal udfyldes.", "Valideringsfejl");
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
                LoadProductsAsync();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
