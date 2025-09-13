using System;
using System.Collections.Generic;
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

        public Renter(string renterName, string renterTelephoneNumber)
        {
            RenterName = renterName;
            RenterTelephoneNumber = renterTelephoneNumber;
            RenterEmail = "0";
        }

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
    }
}
