using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    class Renter
    {
        int _renterId;
        string _renterName;
        string _renterTelephoneNumber;

        public Renter(int renterId, string renterName, string renterTelephoneNumber)
        {
            _renterId = renterId;
            _renterName = renterName;
            _renterTelephoneNumber = renterTelephoneNumber;
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
    }
}
