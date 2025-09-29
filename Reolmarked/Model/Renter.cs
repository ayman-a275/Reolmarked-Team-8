using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class Renter
    {
        int _renterId;
        string _renterName;
        string _renterTelephoneNumber;
        string _renterEmail;
        string _renterAccountNumber;

        public Renter(string renterName, string renterTelephoneNumber)
        {
            RenterName = renterName;
            RenterTelephoneNumber = renterTelephoneNumber;
            RenterEmail = "0";
            RenterAccountNumber = "";
        }

        [Key]
        public int RenterId
        {
            get => _renterId;
            set
            {
                _renterId = value;
            }
        }

        public string RenterName
        {
            get => _renterName;
            set
            {
                _renterName = value;
            }
        }

        public string RenterTelephoneNumber
        {
            get => _renterTelephoneNumber;
            set
            {
                _renterTelephoneNumber = value;
            }
        }

        public string RenterEmail
        {
            get => _renterEmail;
            set
            {
                _renterEmail = value;
            }
        }

        public string RenterAccountNumber
        {
            get => _renterAccountNumber;
            set
            {
                _renterAccountNumber = value;
            }
        }
    }
}