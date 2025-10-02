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
using Microsoft.Extensions.Configuration;
using Reolmarked.Command;
using Reolmarked.Helper;
using Reolmarked.Model;
using Reolmarked.View;

namespace Reolmarked.ViewModel
{
    public class EditShelfViewModel : INotifyPropertyChanged
    {
        private Shelf _shelfToEdit;
        private string _renterName;
        private decimal _agreedPrice;
        private bool _isShelfRented;
        private int _selectedShelfTypeId;

        public ObservableCollection<ShelfType> ShelfTypes { get; set; }
        public ICommand SaveChangesCommand { get; }
        public ICommand CancelCommand { get; }
        public event EventHandler RequestClose;

        public Shelf ShelfToEdit
        {
            get => _shelfToEdit;
            set
            {
                _shelfToEdit = value;
                OnPropertyChanged();
                if (_shelfToEdit != null)
                {
                    SelectedShelfTypeId = _shelfToEdit.ShelfTypeId;
                }
                LoadRentalInfo();
            }
        }

        public int SelectedShelfTypeId
        {
            get => _selectedShelfTypeId;
            set
            {
                _selectedShelfTypeId = value;
                OnPropertyChanged();
                if (_shelfToEdit != null)
                {
                    _shelfToEdit.ShelfTypeId = value;
                }
            }
        }

        public string RenterName
        {
            get => _renterName;
            set
            {
                _renterName = value;
                OnPropertyChanged();
            }
        }

        public decimal AgreedPrice
        {
            get => _agreedPrice;
            set
            {
                _agreedPrice = value;
                OnPropertyChanged();
            }
        }

        public bool IsShelfRented
        {
            get => _isShelfRented;
            set
            {
                _isShelfRented = value;
                OnPropertyChanged();
            }
        }

        public EditShelfViewModel(Shelf shelf)
        {
            ShelfToEdit = shelf;
            LoadShelfTypes();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadShelfTypes()
        {
            try
            {
                using var context = DbContextFactory.CreateContext();
                ShelfTypes = new ObservableCollection<ShelfType>(context.ShelfType.ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading shelf types: {ex.Message}");
                ShelfTypes = new ObservableCollection<ShelfType>();
            }
        }

        private void LoadRentalInfo()
        {
            IsShelfRented = ShelfToEdit.ShelfRented;

            if (IsShelfRented)
            {
                try
                {
                    using var context = DbContextFactory.CreateContext();
                    var rentedShelf = context.RentedShelf.FirstOrDefault(r => r.ShelfNumber == ShelfToEdit.ShelfNumber);

                    if (rentedShelf != null)
                    {
                        var renter = context.Renter.FirstOrDefault(r => r.RenterId == rentedShelf.RenterId);
                        RenterName = renter?.RenterName ?? "Ukendt lejer";
                        AgreedPrice = rentedShelf.RentedShelfAgreedPrice;
                    }
                    else
                    {
                        RenterName = "Ingen lejer fundet";
                        AgreedPrice = 0;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading rental info: {ex.Message}");
                    RenterName = "Fejl ved indlæsning";
                    AgreedPrice = 0;
                }
            }
            else
            {
                RenterName = "";
                AgreedPrice = 0;
            }
        }

        private void SaveChanges()
        {
            try
            {
                using var context = DbContextFactory.CreateContext();
                var shelfToUpdate = context.Shelf.FirstOrDefault(r => r.ShelfNumber == ShelfToEdit.ShelfNumber);

                if (shelfToUpdate != null)
                {
                    shelfToUpdate.ShelfTypeId = SelectedShelfTypeId;

                    context.SaveChanges();

                    MessageBox.Show("Ændringer gemt succesfuldt!", "Succès",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                    OnPropertyChanged();
                    OnRequestClose();
                }
                else
                {
                    MessageBox.Show("Kunne ikke finde reol i databasen.", "Fejl",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved gemning af ændringer: {ex.Message}", "Fejl",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error saving shelf changes: {ex.Message}");
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