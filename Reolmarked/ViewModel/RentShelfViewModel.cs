using Reolmarked.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Reolmarked.Model;
using Microsoft.Extensions.Configuration;
using Reolmarked.Helper;
using System.Windows;

namespace Reolmarked.ViewModel
{
    public class RentShelfViewModel : INotifyPropertyChanged
    {
        private string _BtnShelfColor = "#00a67d";
        private bool _BtnShelfBool;
        private decimal _rentedAgreedPrice;
        private Renter? _selectedRenter;
        public ObservableCollection<Renter> Renters { get; set; }
        public ObservableCollection<AgreementLine> AgreementLine { get; set; }
        public ObservableCollection<Shelf> Shelfs { get; set; }
        public ObservableCollection<BookingShelf> BookingShelfs { get; set; }

        public string BtnShelfColor
        {
            get => _BtnShelfColor;
            set
            {
                _BtnShelfColor = value;
                OnPropertyChanged();
            }
        }

        public bool BtnShelfBool
        {
            get => _BtnShelfBool;
            set
            {
                _BtnShelfBool = value;
                OnPropertyChanged();
            }
        }

        public decimal RentedAgreedPrice
        {
            get => _rentedAgreedPrice;
            set
            {
                _rentedAgreedPrice = value;
                OnPropertyChanged();
            }
        }

        public Renter? SelectedRenter
        {
            get => _selectedRenter;
            set
            {
                _selectedRenter = value;
                OnPropertyChanged();
            }
        }

        public ICommand BtnShelfClickedCommand { get; }
        public ICommand BtnBookShelfClickedCommand { get; }

        public RentShelfViewModel()
        {
            using var context = DbContextFactory.CreateContext();
            Renters = new ObservableCollection<Renter>(context.Renter.ToList());
            AgreementLine = new ObservableCollection<AgreementLine>(context.AgreementLine.ToList());
            Shelfs = new ObservableCollection<Shelf>(context.Shelf.ToList());
            BtnShelfClickedCommand = new RelayCommand(BtnShelfClicked, canExecuteBtnShelfClicked);
            BtnBookShelfClickedCommand = new RelayCommand(BtnBookShelfClicked);
            BookingShelfs = new ObservableCollection<BookingShelf>();

            foreach (Shelf shelf in Shelfs)
            {
                if (shelf.ShelfRented)
                    BookingShelfs.Add(new BookingShelf(shelf.ShelfNumber, true));
                else
                    BookingShelfs.Add(new BookingShelf(shelf.ShelfNumber, false));
            }
        }

        public void BtnShelfClicked(object? index)
        {
            int indexNum = Convert.ToInt32(index);
            var shelf = BookingShelfs[indexNum];
            shelf.ShelfClicked = !shelf.ShelfClicked;
        }

        public bool canExecuteBtnShelfClicked(object? index)
        {
            using var context = DbContextFactory.CreateContext();
            int indexNum = Convert.ToInt32(index);
            var shelf = BookingShelfs[indexNum];
            Shelf shelfToCheck = context.Shelf.FirstOrDefault(r => r.ShelfNumber == shelf.ShelfNumber);
            if (shelfToCheck.ShelfRented)
                return false;
            return true;
        }

        public void BtnBookShelfClicked()
        {
            if (SelectedRenter == null)
            {
                MessageBox.Show("Du skal vælge en lejer.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!BookingShelfs.Any(b => b.ShelfClicked))
            {
                MessageBox.Show("Du skal have valgt mindst 1 reol.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RentedAgreedPrice <= 0)
            {
                MessageBox.Show("Aftalt pris skal være større end 0.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using var context = DbContextFactory.CreateContext();

                var selectedShelfs = BookingShelfs.Where(b => b.ShelfClicked).ToList();

                RentalAgreement newRentalAgreement = new RentalAgreement(
                    SelectedRenter.RenterId,
                    DateTime.Now,
                    RentedAgreedPrice
                );

                context.RentalAgreement.Add(newRentalAgreement);
                context.SaveChanges();

                foreach (BookingShelf bookingShelf in selectedShelfs)
                {
                    Shelf shelfToChange = context.Shelf.FirstOrDefault(r => r.ShelfNumber == bookingShelf.ShelfNumber);
                    if (shelfToChange != null)
                    {
                        shelfToChange.ShelfRented = true;

                        AgreementLine newAgreementLine = new AgreementLine(
                            bookingShelf.ShelfNumber,
                            newRentalAgreement.RentalAgreementId
                        );
                        context.AgreementLine.Add(newAgreementLine);
                    }
                }

                context.SaveChanges();

                MessageBox.Show($"Lejeaftale oprettet succesfuldt!\n\nLejer: {SelectedRenter.RenterName}\nAntal reoler: {selectedShelfs.Count}\nAftalt pris: {RentedAgreedPrice:C}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (var bookingShelf in selectedShelfs)
                {
                    bookingShelf.ShelfClicked = false;
                    bookingShelf.ShelfBooked = true;
                }

                RentedAgreedPrice = 0;
                SelectedRenter = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show( $"Fejl ved oprettelse af lejeaftale: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}