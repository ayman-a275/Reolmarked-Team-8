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
    public class RentalAgreement : INotifyPropertyChanged
    {
        private int _rentalAgreementId;
        private int _renterId;
        private DateTime _rentalAgreementDate;
        private decimal _rentalAgreementTotalPrice;

        public RentalAgreement(int renterId, DateTime rentalAgreementDate, decimal rentalAgreementTotalPrice)
        {
            RenterId = renterId;
            RentalAgreementDate = rentalAgreementDate;
            RentalAgreementTotalPrice = rentalAgreementTotalPrice;
        }

        [Key]
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
