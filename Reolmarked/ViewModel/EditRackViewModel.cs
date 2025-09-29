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
using Reolmarked.Data;
using Reolmarked.Model;
using Reolmarked.View;

namespace Reolmarked.ViewModel
{
    public class EditRackViewModel : INotifyPropertyChanged
    {
        private Rack _rackToEdit;
        private string _renterName;
        private decimal _agreedPrice;
        private bool _isRackRented;
        private int _selectedRackTypeId;

        public ObservableCollection<RackType> RackTypes { get; set; }
        public ICommand SaveChangesCommand { get; }
        public ICommand CancelCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");
        public event EventHandler RequestClose;

        public Rack RackToEdit
        {
            get => _rackToEdit;
            set
            {
                _rackToEdit = value;
                OnPropertyChanged();
                if (_rackToEdit != null)
                {
                    SelectedRackTypeId = _rackToEdit.RackTypeId;
                }
                LoadRentalInfo();
            }
        }

        public int SelectedRackTypeId
        {
            get => _selectedRackTypeId;
            set
            {
                _selectedRackTypeId = value;
                OnPropertyChanged();
                if (_rackToEdit != null)
                {
                    _rackToEdit.RackTypeId = value;
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

        public bool IsRackRented
        {
            get => _isRackRented;
            set
            {
                _isRackRented = value;
                OnPropertyChanged();
            }
        }

        public EditRackViewModel(Rack rack)
        {
            RackToEdit = rack;
            LoadRackTypes();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadRackTypes()
        {
            try
            {
                using var context = new AppDbContext(connectionString);
                RackTypes = new ObservableCollection<RackType>(context.RackType.ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading rack types: {ex.Message}");
                RackTypes = new ObservableCollection<RackType>();
            }
        }

        private void LoadRentalInfo()
        {
            IsRackRented = RackToEdit.RackRented;

            if (IsRackRented)
            {
                try
                {
                    using var context = new AppDbContext(connectionString);
                    var rentedRack = context.RentedRack.FirstOrDefault(r => r.RackNumber == RackToEdit.RackNumber);

                    if (rentedRack != null)
                    {
                        var renter = context.Renter.FirstOrDefault(r => r.RenterId == rentedRack.RenterId);
                        RenterName = renter?.RenterName ?? "Ukendt lejer";
                        AgreedPrice = rentedRack.RentedRackAgreedPrice;
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
                using var context = new AppDbContext(connectionString);
                var rackToUpdate = context.Rack.FirstOrDefault(r => r.RackNumber == RackToEdit.RackNumber);

                if (rackToUpdate != null)
                {
                    rackToUpdate.RackTypeId = SelectedRackTypeId;

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
                System.Diagnostics.Debug.WriteLine($"Error saving rack changes: {ex.Message}");
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