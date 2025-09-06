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
    public class RackViewModel
    {
        public ObservableCollection<Rack> Racks { get; set; }
        private int _rackNumber;
        private decimal _rackPrice;
        public ICommand AddRackBtnClickCommand { get; }
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

        public decimal RackPrice
        {
            get => _rackPrice;
            set
            {
                _rackPrice = value;
                OnPropertyChanged();
            }
        }

        public RackViewModel()
        {
            using var context = new AppDbContext(connectionString);
            Racks = new ObservableCollection<Rack>(context.Rack.ToList());
            AddRackBtnClickCommand = new RelayCommand(AddRackBtnClick);
        }

        private async void AddRackBtnClick()
        {
            var newRack = new Rack(RackNumber, RackPrice);
            Racks.Add(newRack);

            try
            {
                await Task.Run(() =>
                {
                    var dbRack = new Rack(RackNumber, RackPrice);

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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding rack to database: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
