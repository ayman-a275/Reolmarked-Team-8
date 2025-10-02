using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class BookingShelf : INotifyPropertyChanged
    {
        private int _shelfNumber;
        private bool _shelfBooked;
        private bool _shelfClicked;
        private string _shelfColor;

        public BookingShelf(int shelfNumber, bool shelfBooked)
        {
            ShelfNumber = shelfNumber;
            ShelfColor = "#00a67d";
            ShelfBooked = shelfBooked;
            ShelfClicked = false;
        }

        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
                OnPropertyChanged();
            }
        }

        public bool ShelfBooked
        {
            get => _shelfBooked;
            set
            {
                _shelfBooked = value;
                if(ShelfBooked)
                    ShelfColor = "#cb2d6f";
                OnPropertyChanged();
            }
        }

        public bool ShelfClicked
        {
            get => _shelfClicked;
            set
            {
                _shelfClicked = value;
                if (ShelfClicked && !ShelfBooked)
                    ShelfColor = "#0600d1";
                if (!ShelfClicked && !ShelfBooked)
                    ShelfColor = "#00a67d";
                OnPropertyChanged();
            }
        }

        public string ShelfColor
        {
            get => _shelfColor;
            set
            {
                _shelfColor = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
