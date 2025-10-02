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
    public class ShelfWithTypeName : INotifyPropertyChanged
    {
        private Shelf _shelf;
        private string _shelfTypeName;
        private decimal _shelfTypePrice;
        private decimal? _agreedPrice;

        public ShelfWithTypeName(Shelf shelf, string shelfTypeName, decimal shelfTypePrice, decimal? agreedPrice = null)
        {
            _shelf = shelf;
            _shelfTypeName = shelfTypeName;
            _shelfTypePrice = shelfTypePrice;
            _agreedPrice = agreedPrice;
        }

        public Shelf Shelf
        {
            get => _shelf;
            set
            {
                _shelf = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayPrice));
            }
        }

        public int ShelfNumber => _shelf.ShelfNumber;
        public bool ShelfRented => _shelf.ShelfRented;
        public int ShelfTypeId => _shelf.ShelfTypeId;

        public string ShelfTypeName
        {
            get => _shelfTypeName;
            set
            {
                _shelfTypeName = value;
                OnPropertyChanged();
            }
        }

        public decimal ShelfTypePrice
        {
            get => _shelfTypePrice;
            set
            {
                _shelfTypePrice = value;
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

        public decimal DisplayPrice => ShelfRented && AgreedPrice.HasValue ? AgreedPrice.Value : ShelfTypePrice;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class ShelfViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ShelfWithTypeName> Shelfs { get; set; }
        public ObservableCollection<ShelfType> ShelfTypes { get; set; }
        private int _shelfNumber;
        private int _selectedShelfTypeId;

        public ICommand AddShelfBtnClickCommand { get; }
        public ICommand EditShelfCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
                OnPropertyChanged();
            }
        }

        public int SelectedShelfTypeId
        {
            get => _selectedShelfTypeId;
            set
            {
                _selectedShelfTypeId = value;
                OnPropertyChanged();
            }
        }

        public ShelfViewModel()
        {
            LoadData();
            AddShelfBtnClickCommand = new RelayCommand(AddShelfBtnClick);
            EditShelfCommand = new RelayCommand(EditShelf);
        }

        private void LoadData()
        {
            using var context = new AppDbContext(connectionString);

            ShelfTypes = new ObservableCollection<ShelfType>(context.ShelfType.ToList());

            if (ShelfTypes.Any())
            {
                SelectedShelfTypeId = ShelfTypes.First().ShelfTypeId;
            }

            LoadShelfsWithTypeNames(context);
        }

        private void LoadShelfsWithTypeNames(AppDbContext context)
        {
            var shelfsWithTypes = (from shelf in context.Shelf
                                  join shelfType in context.ShelfType on shelf.ShelfTypeId equals shelfType.ShelfTypeId
                                  select new { shelf, shelfType }).ToList();

            var shelfsWithPrices = new List<ShelfWithTypeName>();

            foreach (var item in shelfsWithTypes)
            {
                decimal? agreedPrice = null;

                if (item.shelf.ShelfRented)
                {
                    var rentedShelf = context.RentedShelf.FirstOrDefault(r => r.ShelfNumber == item.shelf.ShelfNumber);
                    agreedPrice = rentedShelf?.RentedShelfAgreedPrice;
                }

                shelfsWithPrices.Add(new ShelfWithTypeName(item.shelf, item.shelfType.ShelfTypeName, item.shelfType.ShelfTypePrice, agreedPrice));
            }

            Shelfs = new ObservableCollection<ShelfWithTypeName>(shelfsWithPrices);
        }

        private async void AddShelfBtnClick()
        {
            using (var checkContext = new AppDbContext(connectionString))
            {
                if (checkContext.Shelf.Any(r => r.ShelfNumber == ShelfNumber))
                {
                    System.Windows.MessageBox.Show($"Reol nummer {ShelfNumber} eksisterer allerede!", "Fejl",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }

            var selectedShelfType = ShelfTypes.FirstOrDefault(rt => rt.ShelfTypeId == SelectedShelfTypeId);
            var shelfTypeName = selectedShelfType?.ShelfTypeName ?? "Ukendt type";
            var shelfTypePrice = selectedShelfType?.ShelfTypePrice ?? 0;

            var newShelf = new Shelf(ShelfNumber, SelectedShelfTypeId);
            var newShelfWithTypeName = new ShelfWithTypeName(newShelf, shelfTypeName, shelfTypePrice);
            Shelfs.Add(newShelfWithTypeName);

            try
            {
                await Task.Run(() =>
                {
                    var dbShelf = new Shelf(ShelfNumber, SelectedShelfTypeId);

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException("Database connection string is null or empty.");
                    }

                    using (var db = new AppDbContext(connectionString))
                    {
                        db.Shelf.Add(dbShelf);
                        db.SaveChanges();
                    }
                });

                ShelfNumber = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding shelf to database: {ex.Message}");
                System.Windows.MessageBox.Show($"Fejl ved tilføjelse af reol: {ex.Message}", "Fejl",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                Shelfs.Remove(newShelfWithTypeName);
            }
        }

        private void EditShelf(object? parameter)
        {
            if (parameter is ShelfWithTypeName shelfWithTypeName)
            {
                var editShelfWindow = new EditShelfWindow(shelfWithTypeName.Shelf);
                editShelfWindow.ShowDialog();
                OnPropertyChanged();
                RefreshShelfsList();
            }
        }

        private void RefreshShelfsList()
        {
            try
            {
                using var context = new AppDbContext(connectionString);
                LoadShelfsWithTypeNames(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error refreshing shelfs list: {ex.Message}");
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