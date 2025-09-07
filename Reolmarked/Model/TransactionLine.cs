using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class TransactionLine : INotifyPropertyChanged
    {
        int _transactionLineId;
        int _transactionId;
        string _productSerialNumber;

        public TransactionLine(int transactionLineId, int transactionId, string productSerialNumber)
        {
            TransactionLineId = transactionLineId;
            TransactionId = transactionId;
            ProductSerialNumber = productSerialNumber;
        }

        public int TransactionLineId
        {
            get => _transactionLineId;
            set
            {
                _transactionLineId = value;
                OnPropertyChanged();
            }
        }

        public int TransactionId
        {
            get => _transactionId;
            set
            {
                _transactionId = value;
                OnPropertyChanged();
            }
        }

        public string ProductSerialNumber
        {
            get => _productSerialNumber;
            set
            {
                _productSerialNumber = value;
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
