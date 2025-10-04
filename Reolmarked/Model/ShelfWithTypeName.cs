using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
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
            }
        }

        public decimal? AgreedPrice
        {
            get => _agreedPrice;
            set
            {
                _agreedPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal DisplayPrice => ShelfRented && AgreedPrice.HasValue ? AgreedPrice.Value : ShelfTypePrice;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
