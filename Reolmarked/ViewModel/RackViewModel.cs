using Microsoft.Extensions.Configuration;
using Reolmarked.Command;
using Reolmarked.Helper;
using Reolmarked.Model;
using Reolmarked.View;
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
    public class RackWithTypeName : INotifyPropertyChanged
    {
        private Rack _rack;
        private string _rackTypeName;
        private decimal _rackTypePrice;
        private decimal? _agreedPrice;

        public RackWithTypeName(Rack rack, string rackTypeName, decimal rackTypePrice, decimal? agreedPrice = null)
        {
            _rack = rack;
            _rackTypeName = rackTypeName;
            _rackTypePrice = rackTypePrice;
            _agreedPrice = agreedPrice;
        }

        public Rack Rack
        {
            get => _rack;
            set
            {
                _rack = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayPrice));
            }
        }

        public int RackNumber => _rack.RackNumber;
        public bool RackRented => _rack.RackRented;
        public int RackTypeId => _rack.RackTypeId;

        public string RackTypeName
        {
            get => _rackTypeName;
            set
            {
                _rackTypeName = value;
                OnPropertyChanged();
            }
        }

        public decimal RackTypePrice
        {
            get => _rackTypePrice;
            set
            {
                _rackTypePrice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayPrice));
            }
        }

        public decimal? AgreedPrice
        {
            get => _agreedPrice;
            set
            {
                _agreedPrice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayPrice));
            }
        }

        public decimal DisplayPrice => RackRented && AgreedPrice.HasValue ? AgreedPrice.Value : RackTypePrice;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class RackViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RackWithTypeName> Racks { get; set; }
        public ObservableCollection<RackType> RackTypes { get; set; }
        private int _rackNumber;
        private int _selectedRackTypeId;

        public ICommand AddRackBtnClickCommand { get; }
        public ICommand EditRackCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRackTypeId
        {
            get => _selectedRackTypeId;
            set
            {
                _selectedRackTypeId = value;
                OnPropertyChanged();
            }
        }

        public RackViewModel()
        {
            LoadData();
            AddRackBtnClickCommand = new RelayCommand(AddRackBtnClick);
            EditRackCommand = new RelayCommand(EditRack);
        }

        private void LoadData()
        {
            using var context = new AppDbContext(connectionString);

            RackTypes = new ObservableCollection<RackType>(context.RackType.ToList());

            if (RackTypes.Any())
            {
                SelectedRackTypeId = RackTypes.First().RackTypeId;
            }

            LoadRacksWithTypeNames(context);
        }

        private void LoadRacksWithTypeNames(AppDbContext context)
        {
            var racksWithTypes = (from rack in context.Rack
                                  join rackType in context.RackType on rack.RackTypeId equals rackType.RackTypeId
                                  select new { rack, rackType }).ToList();

            var racksWithPrices = new List<RackWithTypeName>();

            foreach (var item in racksWithTypes)
            {
                decimal? agreedPrice = null;

                if (item.rack.RackRented)
                {
                    var rentedRack = context.RentedRack.FirstOrDefault(r => r.RackNumber == item.rack.RackNumber);
                    agreedPrice = rentedRack?.RentedRackAgreedPrice;
                }

                racksWithPrices.Add(new RackWithTypeName(item.rack, item.rackType.RackTypeName, item.rackType.RackTypePrice, agreedPrice));
            }

            Racks = new ObservableCollection<RackWithTypeName>(racksWithPrices);
        }

        private async void AddRackBtnClick()
        {
            using (var checkContext = new AppDbContext(connectionString))
            {
                if (checkContext.Rack.Any(r => r.RackNumber == RackNumber))
                {
                    System.Windows.MessageBox.Show($"Reol nummer {RackNumber} eksisterer allerede!", "Fejl",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }

            var selectedRackType = RackTypes.FirstOrDefault(rt => rt.RackTypeId == SelectedRackTypeId);
            var rackTypeName = selectedRackType?.RackTypeName ?? "Ukendt type";
            var rackTypePrice = selectedRackType?.RackTypePrice ?? 0;

            var newRack = new Rack(RackNumber, SelectedRackTypeId);
            var newRackWithTypeName = new RackWithTypeName(newRack, rackTypeName, rackTypePrice);
            Racks.Add(newRackWithTypeName);

            try
            {
                await Task.Run(() =>
                {
                    var dbRack = new Rack(RackNumber, SelectedRackTypeId);

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException("Database connection string is null or empty.");
                    }

                    using (var db = new AppDbContext(connectionString))
                    {
                        db.Rack.Add(dbRack);
                        db.SaveChanges();
                    }
                });

                RackNumber = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding rack to database: {ex.Message}");
                System.Windows.MessageBox.Show($"Fejl ved tilføjelse af reol: {ex.Message}", "Fejl",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                Racks.Remove(newRackWithTypeName);
            }
        }

        private void EditRack(object? parameter)
        {
            if (parameter is RackWithTypeName rackWithTypeName)
            {
                var editRackWindow = new EditRackWindow(rackWithTypeName.Rack);
                editRackWindow.ShowDialog();
                OnPropertyChanged();
                RefreshRacksList();
            }
        }

        private void RefreshRacksList()
        {
            try
            {
                using var context = new AppDbContext(connectionString);
                LoadRacksWithTypeNames(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error refreshing racks list: {ex.Message}");
                System.Windows.MessageBox.Show($"Fejl ved opdatering af reolliste: {ex.Message}", "Fejl",
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