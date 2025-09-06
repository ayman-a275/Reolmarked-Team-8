using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    class Transaction
    {
        int _transactionId;
        DateTime _transactionDateTime;
        decimal _transactionPayment;
        string _transactionPaymentMethod;
        decimal _transactionChange;

        public Transaction(int transactionId, DateTime transactionDateTime, decimal transactionPayment, string transactionPaymentMethod, decimal transactionChange)
        {
            _transactionId = transactionId;
            _transactionDateTime = transactionDateTime;
            _transactionPayment = transactionPayment;
            _transactionPaymentMethod = transactionPaymentMethod;
            _transactionChange = transactionChange;
        }

        public int TransactionId
        {
            get => _transactionId;
            set
            {
                _transactionId = value;
            }
        }

        public DateTime TransactionDateTime
        {
            get => _transactionDateTime;
            set
            {
                _transactionDateTime = value;
            }
        }

        public string TransactionPaymentMethod
        {
            get => _transactionPaymentMethod;
            set
            {
                _transactionPaymentMethod = value;
            }
        }

        public decimal TransactionChange
        {
            get => _transactionChange;
            set
            {
                _transactionChange = value;
            }
        }
    }
}
