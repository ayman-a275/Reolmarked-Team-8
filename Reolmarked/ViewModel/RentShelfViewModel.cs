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
        public ObservableCollection<RentalAgreement> RentedShelfs { get; set; }
        public ObservableCollection<Shelf> Shelfs { get; set; }
        public ObservableCollection<BookingShelf> BookingShelfs { get; set; }

        public string BtnShelfColor { 
            get => _BtnShelfColor; 
            set {
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
            RentedShelfs = new ObservableCollection<RentalAgreement>(context.RentedShelf.ToList());
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
            if (!BookingShelfs.Any(b => b.ShelfClicked))
            {
                MessageBox.Show("Du skal have valgt mindst 1 reol.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RentalAgreement newRentedShelf;
            Shelf shelfToChange;
            decimal calculatedPrice = RentedAgreedPrice / BookingShelfs.Count;

            using var context = DbContextFactory.CreateContext();
            foreach (BookingShelf bookingShelf in BookingShelfs)
            {
                if (bookingShelf.ShelfClicked)
                {
                    shelfToChange = context.Shelf.FirstOrDefault(r => r.ShelfNumber == bookingShelf.ShelfNumber);
                    newRentedShelf = new RentalAgreement(bookingShelf.ShelfNumber, SelectedRenter.RenterId, calculatedPrice);
                    context.RentedShelf.Add(newRentedShelf);
                    shelfToChange.ShelfRented = true;
                    context.SaveChanges();
                }

            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
