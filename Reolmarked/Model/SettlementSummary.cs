using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Reolmarked.Model
{
    public class SettlementSummary : INotifyPropertyChanged
    {
        private int _renterId;
        private string _renterName;
        private string _renterAccountNumber;
        private decimal _totalRent;
        private decimal _totalSales;
        private decimal _commission;
        private decimal _netAmount;
        private bool _isPaid;

        public SettlementSummary(int renterId, string renterName, string renterAccountNumber, decimal totalRent, decimal totalSales, decimal commission, bool isPaid = false)
        {
            RenterId = renterId;
            RenterName = renterName;
            RenterAccountNumber = renterAccountNumber;
            TotalRent = totalRent;
            TotalSales = totalSales;
            Commission = commission;
            NetAmount = totalSales - commission - totalRent;
            IsPaid = isPaid;
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

        public string RenterAccountNumber
        {
            get => _renterAccountNumber;
            set
            {
                _renterAccountNumber = value;
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
                UpdateNetAmount();
            }
        }

        public decimal TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                OnPropertyChanged();
                UpdateNetAmount();
            }
        }

        public decimal Commission
        {
            get => _commission;
            set
            {
                _commission = value;
                OnPropertyChanged();
                UpdateNetAmount();
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

        private void UpdateNetAmount()
        {
            NetAmount = TotalSales - Commission - TotalRent;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}