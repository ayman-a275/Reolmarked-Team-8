using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Reolmarked.Command;
using Reolmarked.Helper;
using Reolmarked.Model;

namespace Reolmarked.ViewModel
{
    public class EditProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Rack> Racks { get; set; }
        private int _selectedRackNumber;
        private Product _productToEdit;
        private string _productDescription;
        private decimal _productPrice;
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand CancelCommand { get; }
        public event EventHandler RequestClose;

        public Product ProductToEdit
        {
            get => _productToEdit;
            set
            {
                _productToEdit = value;
                OnPropertyChanged();
                LoadProductInfo();
            }
        }

        public int SelectedRackNumber
        {
            get => _selectedRackNumber;
            set
            {
                _selectedRackNumber = value;
                OnPropertyChanged();
            }
        }

        public string ProductDescription
        {
            get => _productDescription;
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

        public EditProductViewModel(Product product)
        {
            using var context = DbContextFactory.CreateContext();
            Racks = new ObservableCollection<Rack>(context.Rack.ToList());

            ProductToEdit = product;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadProductInfo()
        {
            ProductDescription = ProductToEdit.ProductDescription;
            ProductPrice = ProductToEdit.ProductPrice;
            SelectedRackNumber = ProductToEdit.RackNumber;
        }

        private void SaveChanges()
        {
            try
            {
                using var context = DbContextFactory.CreateContext();
                var productToUpdate = context.Product.FirstOrDefault(p => p.ProductSerialNumber == ProductToEdit.ProductSerialNumber);

                if (productToUpdate != null)
                {
                    productToUpdate.ProductDescription = ProductDescription;
                    productToUpdate.ProductPrice = ProductPrice;
                    productToUpdate.RackNumber = SelectedRackNumber;

                    context.SaveChanges();

                    MessageBox.Show("Ændringer gemt succesfuldt!", "Succes",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                    OnPropertyChanged();
                    OnRequestClose();
                }
                else
                {
                    MessageBox.Show("Kunne ikke finde produkt i databasen.", "Fejl",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af ændringer: {ex.Message}", "Fejl",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error saving product changes: {ex.Message}");
            }
        }

        private void DeleteProduct()
        {
            var result = MessageBox.Show("Er du sikker på at du vil slette dette produkt? Denne handling kan ikke fortrydes.",
                                       "Bekræft sletning",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var context = DbContextFactory.CreateContext();
                    var productToDelete = context.Product.FirstOrDefault(p => p.ProductSerialNumber == ProductToEdit.ProductSerialNumber);

                    if (productToDelete != null)
                    {
                        context.Product.Remove(productToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Produkt slettet succesfuldt!", "Succès",
                                        MessageBoxButton.OK, MessageBoxImage.Information);

                        OnRequestClose();
                    }
                    else
                    {
                        MessageBox.Show("Kunne ikke finde produkt i databasen.", "Fejl",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fejl ved sletning af produkt: {ex.Message}", "Fejl",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Diagnostics.Debug.WriteLine($"Error deleting product: {ex.Message}");
                }
            }
        }

        private void Cancel()
        {
            var result = MessageBox.Show("Er du sikker på at du vil annullere? Alle ikke-gemte ændringer vil gå tabt.",
                                       "Bekræft annullering",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                OnRequestClose();
            }
        }

        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}