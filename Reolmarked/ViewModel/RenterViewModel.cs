using Microsoft.EntityFrameworkCore.Internal;
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
using System.Windows;

namespace Reolmarked.ViewModel
{
    public class RenterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Renter> Renters { get; set; }
        private string _renterName;
        private string _renterTelephoneNumber;
        private string _renterEmail;
        private string _renterAccountNumber;
        public ICommand AddRenterBtnClickCommand { get; }

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

        public string RenterEmail
        {
            get => _renterEmail;
            set
            {
                _renterEmail = value;
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
            using var context = DbContextFactory.CreateContext();
            Renters = new ObservableCollection<Renter>(context.Renter.ToList());
            AddRenterBtnClickCommand = new RelayCommand(AddRenterBtnClick);
        }

        private async void AddRenterBtnClick()
        {
            if (string.IsNullOrWhiteSpace(RenterName))
            {
                MessageBox.Show("Lejers navn skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (string.IsNullOrWhiteSpace(RenterTelephoneNumber))
            {
                MessageBox.Show("Lejers tlf. nr. skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (string.IsNullOrWhiteSpace(RenterEmail))
            {
                MessageBox.Show("Lejers e-mail skal udfyldes.", "Valideringsfejl");
                return;
            }

            if (string.IsNullOrWhiteSpace(RenterAccountNumber))
            {
                MessageBox.Show("Lejers reg. og konto nr. skal udfyldes.", "Valideringsfejl");
                return;
            }

            var newRenter = new Renter(RenterName, RenterTelephoneNumber, RenterEmail, RenterAccountNumber);
            try
            {
                await Task.Run(() =>
                {
                    using var context = DbContextFactory.CreateContext();
                    context.Renter.Add(newRenter);
                    context.SaveChanges();
                });

                Renters.Add(newRenter);

                MessageBox.Show("Lejer gemt succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                RenterName = string.Empty;
                RenterTelephoneNumber = string.Empty;
                RenterEmail = string.Empty;
                RenterAccountNumber = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved tilføjelse af lejer: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}