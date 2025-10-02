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

namespace Reolmarked.ViewModel
{
    public class RentShelfViewModel : INotifyPropertyChanged
    {
        private string _BtnShelfColor = "#00a67d";
        private bool _BtnShelfBool;
        public ObservableCollection<Renter> Renters { get; set; }
        public ObservableCollection<RentedShelf> RentedShelfs { get; set; }
        public ObservableCollection<Shelf> Shelfs { get; set; }
        public ObservableCollection<BookingShelf> BookingShelfs { get; set; }
        public static string connectionString = App.Configuration.GetConnectionString("DefaultConnection");

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

        public ICommand BtnShelfClickedCommand { get; }
        public ICommand BtnBookShelfClickedCommand { get; }

        public RentShelfViewModel()
        {
            using var context = new AppDbContext(connectionString);
            Renters = new ObservableCollection<Renter>(context.Renter.ToList());
            RentedShelfs = new ObservableCollection<RentedShelf>(context.RentedShelf.ToList());
            Shelfs = new ObservableCollection<Shelf>(context.Shelf.ToList());
            BtnShelfClickedCommand = new RelayCommand(BtnShelfClicked, canExecuteBtnShelfClicked);
            BtnBookShelfClickedCommand = new RelayCommand(BtnBookShelfClicked, canExecuteBtnBookShelfClicked);
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
            using var context = new AppDbContext(connectionString);
            int indexNum = Convert.ToInt32(index);
            var shelf = BookingShelfs[indexNum];
            Shelf shelfToCheck = context.Shelf.FirstOrDefault(r => r.ShelfNumber == shelf.ShelfNumber);
            if (shelfToCheck.ShelfRented)
                return false;
            return true;
        }

        public void BtnBookShelfClicked()
        {
            using var context = new AppDbContext(connectionString);
            foreach (BookingShelf bookingShelf in BookingShelfs)
            {
                if (bookingShelf.ShelfClicked)
                {
                    Shelf shelfToChange = context.Shelf.FirstOrDefault(r => r.ShelfNumber == bookingShelf.ShelfNumber);
                    shelfToChange.ShelfRented = true;
                    context.SaveChanges();
                }

            }
        }

        public bool canExecuteBtnBookShelfClicked()
        {
            return true;
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
