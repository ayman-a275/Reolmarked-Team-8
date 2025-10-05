using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Reolmarked.Model
{
    public class RentalAgreementWithDetails : INotifyPropertyChanged
    {
        private int _rentalAgreementId;
        private int _renterId;
        private string _renterName;
        private DateTime _rentalAgreementDate;
        private decimal _rentalAgreementTotalPrice;
        private List<int> _shelfNumbers;

        public RentalAgreementWithDetails(int rentalAgreementId, int renterId, string renterName, DateTime rentalAgreementDate, decimal rentalAgreementTotalPrice, List<int> shelfNumbers)
        {
            RentalAgreementId = rentalAgreementId;
            RenterId = renterId;
            RenterName = renterName;
            RentalAgreementDate = rentalAgreementDate;
            RentalAgreementTotalPrice = rentalAgreementTotalPrice;
            ShelfNumbers = shelfNumbers;
        }

        public int RentalAgreementId
        {
            get => _rentalAgreementId;
            set
            {
                _rentalAgreementId = value;
                OnPropertyChanged();
            }
        }

        public int RenterId
        {
            get => _renterId;
            set
            {
                _renterId = value;
                OnPropertyChanged();
            }
        }

        public string RenterName
        {
            get => _renterName;
            set
            {
                _renterName = value;
                OnPropertyChanged();
            }
        }

        public DateTime RentalAgreementDate
        {
            get => _rentalAgreementDate;
            set
            {
                _rentalAgreementDate = value;
                OnPropertyChanged();
            }
        }

        public decimal RentalAgreementTotalPrice
        {
            get => _rentalAgreementTotalPrice;
            set
            {
                _rentalAgreementTotalPrice = value;
                OnPropertyChanged();
            }
        }

        public List<int> ShelfNumbers
        {
            get => _shelfNumbers;
            set
            {
                _shelfNumbers = value;
                OnPropertyChanged();
            }
        }

        public int ShelfCount => ShelfNumbers?.Count ?? 0;

        public string ShelfNumbersDisplay => ShelfNumbers != null && ShelfNumbers.Any()
            ? string.Join(", ", ShelfNumbers.OrderBy(s => s))
            : "Ingen reoler";

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}