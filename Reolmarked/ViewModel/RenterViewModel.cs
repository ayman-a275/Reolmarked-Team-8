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
    public class RenterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Renter> Renters { get; set; }
        private string _renterName;
        private string _renterTelephoneNumber;
        private string _renterAccountNumber;
        public ICommand AddRenterBtnClickCommand { get; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

        public string RenterName
        {
            get => _renterName;
            set
            {
                _renterName = value;
                OnPropertyChanged();
            }
        }

        public string RenterTelephoneNumber
        {
            get => _renterTelephoneNumber;
            set
            {
                _renterTelephoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string RenterAccountNumber
        {
            get => _renterAccountNumber;
            set
            {
                _renterAccountNumber = value;
                OnPropertyChanged();
            }
        }

        public RenterViewModel()
        {
            using var context = new AppDbContext(connectionString);
            Renters = new ObservableCollection<Renter>(context.Renter.ToList());
            AddRenterBtnClickCommand = new RelayCommand(AddRenterBtnClick);
        }

        private async void AddRenterBtnClick()
        {
            var newRenter = new Renter(RenterName, RenterTelephoneNumber)
            {
                RenterAccountNumber = RenterAccountNumber
            };
            Renters.Add(newRenter);

            try
            {
                await Task.Run(() =>
                {
                    var dbRenter = new Renter(RenterName, RenterTelephoneNumber)
                    {
                        RenterAccountNumber = RenterAccountNumber
                    };

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException("Database connection string is null or empty.");
                    }

                    using (var db = new AppDbContext(connectionString))
                    {
                        db.Renter.Add(dbRenter);
                        db.SaveChanges();
                    }
                });

                RenterName = string.Empty;
                RenterTelephoneNumber = string.Empty;
                RenterAccountNumber = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding renter to database: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}