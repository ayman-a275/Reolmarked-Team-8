using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Reolmarked.Model
{
    public class MonthlySettlement : INotifyPropertyChanged
    {
        private int _settlementId;
        private int _renterId;
        private DateTime _settlementDate;
        private decimal _totalRent;
        private decimal _totalSales;
        private decimal _commission;
        private decimal _netAmount;
        private bool _isPaid;

        public MonthlySettlement(int renterId, DateTime settlementDate, decimal totalRent, decimal totalSales, decimal commission)
        {
            RenterId = renterId;
            SettlementDate = settlementDate;
            TotalRent = totalRent;
            TotalSales = totalSales;
            Commission = commission;
            NetAmount = totalSales - commission - totalRent;
            IsPaid = false;
        }

        [Key]
        public int SettlementId
        {
            get => _settlementId;
            set
            {
                _settlementId = value;
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

        public DateTime SettlementDate
        {
            get => _settlementDate;
            set
            {
                _settlementDate = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalRent
        {
            get => _totalRent;
            set
            {
                _totalRent = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                OnPropertyChanged();
            }
        }

        public decimal Commission
        {
            get => _commission;
            set
            {
                _commission = value;
                OnPropertyChanged();
            }
        }

        public decimal NetAmount
        {
            get => _netAmount;
            set
            {
                _netAmount = value;
                OnPropertyChanged();
            }
        }

        public bool IsPaid
        {
            get => _isPaid;
            set
            {
                _isPaid = value;
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
