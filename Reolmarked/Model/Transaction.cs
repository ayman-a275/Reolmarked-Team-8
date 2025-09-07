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
        decimal _transactionTotalAmount;
        decimal _transactionPaidAmount;
        int _paymentMethodId;

        public Transaction(DateTime transactionDateTime, decimal transactionTotalAmount, decimal transactionPaidAmount, int paymentMethodId)
        {
            TransactionDateTime = transactionDateTime;
            TransactionTotalAmount = transactionTotalAmount;
            TransactionPaidAmount = transactionPaidAmount;
            PaymentMethodId = paymentMethodId;
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

        public decimal TransactionTotalAmount
        {
            get => _transactionTotalAmount;
            set
            {
                _transactionTotalAmount = value;
            }
        }

        public decimal TransactionPaidAmount
        {
            get => _transactionPaidAmount;
            set
            {
                _transactionPaidAmount = value;
            }
        }

        public int PaymentMethodId
        {
            get => _paymentMethodId;
            set
            {
                _paymentMethodId = value;
            }
        }
    }
}
