using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        private string _statusColor;

        public SettlementSummary(int renterId, string renterName, string renterAccountNumber, decimal totalRent, decimal totalSales, decimal commission)
        {
            RenterId = renterId;
            RenterName = renterName;
            RenterAccountNumber = renterAccountNumber;
            TotalRent = totalRent;
            TotalSales = totalSales;
            Commission = commission;
            NetAmount = totalSales - commission - totalRent;
            StatusColor = NetAmount >= 0 ? "#28a745" : "#dc3545";
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
                StatusColor = value >= 0 ? "#28a745" : "#dc3545";
                OnPropertyChanged();
            }
        }

        public string StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
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

