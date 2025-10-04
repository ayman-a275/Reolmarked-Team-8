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
    public class AgreementLine : INotifyPropertyChanged
    {
        private int _agreementLineId;
        private int _shelfNumber;
        private int _rentalAgreementId;

        public AgreementLine(int shelfNumber, int rentalAgreementId)
        {
            ShelfNumber = shelfNumber;
            RentalAgreementId = rentalAgreementId;
        }

        [Key]
        public int AgreementLineId
        {
            get => _agreementLineId;
            set
            {
                _agreementLineId = value;
                OnPropertyChanged();
            }
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

        public int RentalAgreementId
        {
            get => _rentalAgreementId;
            set
            {
                _rentalAgreementId = value;
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
