using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class Transaction
    {
        int _transactionId;
        DateTime _transactionDateTime;
        decimal _transactionPayment;
        string _transactionPaymentMethod;
        decimal _transactionChange;

        public Transaction(DateTime transactionDateTime, decimal transactionPayment, string transactionPaymentMethod, decimal transactionChange)
        {
            TransactionDateTime = transactionDateTime;
            TransactionPayment = transactionPayment;
            TransactionPaymentMethod = transactionPaymentMethod;
            TransactionChange = transactionChange;
        }

        [Key]
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

        public decimal TransactionPayment
        {
            get => _transactionPayment;
            set
            {
                _transactionPayment = value;
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
