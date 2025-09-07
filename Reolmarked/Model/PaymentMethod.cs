using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class PaymentMethod
    {
        int _paymentMethodId;
        string _paymentMethodName;

        public PaymentMethod(string paymentMethodName)
        {
            PaymentMethodName = paymentMethodName;
        }

        [Key]
        public int PaymentMethodId
        {
            get => _paymentMethodId;
            set
            {
                _paymentMethodId = value;
            }
        }

        public string PaymentMethodName
        {
            get => _paymentMethodName;
            set
            {
                _paymentMethodName = value;
            }
        }

    }
}
